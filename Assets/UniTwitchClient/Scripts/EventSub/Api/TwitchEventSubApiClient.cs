using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UniRx;
using UniTwitchClient.Common;
using UniTwitchClient.EventSub.Api.Models;
using UnityEngine.Networking;

namespace UniTwitchClient.EventSub.Api
{
    public class TwitchEventSubApiClient : ITwitchEventSubApiClient
    {
        public IUniTwitchLogger Logger { get; set; } = new UniTwitchProductionLogger();

        public bool ConnectToLocalCLIServer { get; set; }

        private const string API_DEBUG_URL = "http://localhost:8080/eventsub/subscriptions";
        private const string API_URL = "https://api.twitch.tv/helix/eventsub/subscriptions";

        private EventSubSubscribeRequestBuilder _eventSubSubscribeRequestBuilder;
        private TwitchApiCredentials _apiCredentials;

        public TwitchEventSubApiClient(TwitchApiCredentials apiCredentials)
        {
            _eventSubSubscribeRequestBuilder = new EventSubSubscribeRequestBuilder();
            _apiCredentials = apiCredentials;
        }

        public async UniTask CreateEventSubSubscriptionsAsync(string broadcasterUserId, string sessionId, CancellationToken cancellationToken = default)
        {
            await CreateEventSubSubscriptionsAsync(broadcasterUserId, sessionId, broadcasterUserId);
        }

        public async UniTask CreateEventSubSubscriptionsAsync(string broadcasterUserId, string sessionId, string moderatorUserId, CancellationToken cancellationToken = default)
        {
            _eventSubSubscribeRequestBuilder.CreateAllRequests(broadcasterUserId, moderatorUserId);
            var requests = _eventSubSubscribeRequestBuilder.BuildRequestsWithSessionId(sessionId);

            List<UniTask<bool>> tasks = new List<UniTask<bool>>();
            foreach (var request in requests)
            {
                tasks.Add(CreateEventSubSubscriptionAsync(request, cancellationToken));
            }

            try
            {
                await UniTask.WhenAll(tasks);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private UniTask<bool> CreateEventSubSubscriptionAsync(EventSubSubscribeRequest request, CancellationToken cancellationToken)
        {
            var uwp = CreateEventSubSubscriptionRequest(request);
            uwp.timeout = 7;
            var utcs = new UniTaskCompletionSource<bool>();
            var disposables = new CompositeDisposable();
            disposables.Add(uwp);

            uwp.SendWebRequest().ToUniTask(cancellationToken: cancellationToken).ToObservable().Subscribe(x =>
            {
                Logger.Log($"Successfully subscribe {request.SubscriptionType.ToString()}");
                utcs.TrySetResult(true);
            }, ex =>
            {
                Logger.LogError($"Failed to subscribe {request.SubscriptionType.ToString()}");
                Logger.LogError(ex.Message);
                // 失敗してもタスクは完了させる
                utcs.TrySetResult(true);
                disposables.Dispose();
            }, () =>
            {
                disposables.Dispose();
            }).AddTo(disposables);

            return utcs.Task;
        }

        private UnityWebRequest CreateEventSubSubscriptionRequest(EventSubSubscribeRequest subscription)
        {
            var json = subscription.ToJson();
            Logger.Log("[TwitchEventSubApiClient] Json:" + json);

            var url = ConnectToLocalCLIServer == true ? API_DEBUG_URL : API_URL;
            return CreateUnityWebRequest(url, UnityWebRequest.kHttpVerbPOST, json);
        }

        public async UniTask<EventSubSubscriptionData> GetEventSubSubscriptionsAsync()
        {
            var url = ConnectToLocalCLIServer == true ? API_DEBUG_URL : API_URL;
            var unityWebRequest = CreateUnityWebRequest(url, UnityWebRequest.kHttpVerbGET);

            EventSubSubscriptionData eventSubSubscriptionData = null;
            using (unityWebRequest)
            {
                var result = await unityWebRequest.SendWebRequest().ToUniTask();
                Logger.Log(result.downloadHandler.text);
                eventSubSubscriptionData = JsonWrapper.ConvertFromJson<subscription_data>(result.downloadHandler.text).ConvertRawToModel();
            }

            return eventSubSubscriptionData;
        }

        public async UniTask DeleteEventSubSubscriptionsAsync(EventSubSubscriptionData subscriptions)
        {
            var url = ConnectToLocalCLIServer == true ? API_DEBUG_URL : API_URL;
            List<UnityWebRequest> unityWebRequests = new List<UnityWebRequest>();

            foreach (var subscription in subscriptions.Subscriptions)
            {
                unityWebRequests.Add(CreateUnityWebRequest(url + $"?id={subscription.Id}", UnityWebRequest.kHttpVerbDELETE));
            }

            var tasks = unityWebRequests.Select(x => x.SendWebRequest().ToUniTask());
            try
            {
                await UniTask.WhenAll(tasks);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                unityWebRequests.ForEach(x => x.Dispose());
            }
        }

        public async UniTask DeleteEventSubSubscriptionsAsync(string sessionId)
        {
            if (string.IsNullOrEmpty(sessionId)) return;

            var eventSubSubscriptionData = await GetEventSubSubscriptionsAsync();
            await DeleteEventSubSubscriptionsAsync(eventSubSubscriptionData.GetSubscriptionsBySessionId(sessionId));
        }

        private UnityWebRequest CreateUnityWebRequest(string url, string method, string postData = null)
        {
            byte[] data = null;
            if (method == UnityWebRequest.kHttpVerbPOST)
            {
                data = Encoding.UTF8.GetBytes(postData);
            }

            var uwr = new UnityWebRequest(url, method)
            {
                uploadHandler = new UploadHandlerRaw(data),
                downloadHandler = new DownloadHandlerBuffer()
            };
            uwr.SetRequestHeader("Authorization", _apiCredentials.AuthorizationBearer);
            uwr.SetRequestHeader("Client-Id", _apiCredentials.ClientId);
            if (data != null)
            {
                uwr.SetRequestHeader("Content-type", "application/json");
            }

            return uwr;
        }
    }
}