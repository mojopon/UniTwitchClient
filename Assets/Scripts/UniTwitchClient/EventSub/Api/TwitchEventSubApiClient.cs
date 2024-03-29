using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;
using System.Text;
using System.Threading;
using UniTwitchClient.EventSub.Api.Models;
using System.Linq;

namespace UniTwitchClient.EventSub.Api
{
    public class TwitchEventSubApiClient : ITwitchEventSubApiClient
    {
        public bool DebugMode
        {
            get
            {
                return _debugMode;
            }
            set
            {
                _debugMode = value;
            }
        }

        private const string API_DEBUG_URL = "http://localhost:8080/eventsub/subscriptions";
        private const string API_URL = "https://api.twitch.tv/helix/eventsub/subscriptions";

        private EventSubSubscribeRequestBuilder _subscriptionBuilder;
        private ApiCredentials _apiCredentials;
        private bool _debugMode;

        public TwitchEventSubApiClient(ApiCredentials apiCredentials)
        {
            _subscriptionBuilder = new EventSubSubscribeRequestBuilder();
            _apiCredentials = apiCredentials;
        }

        public async UniTask CreateEventSubSubscriptionsAsync(string broadcasterUserId, string sessionId, string moderatorUserId = null)
        {
            if (string.IsNullOrEmpty(moderatorUserId)) 
            {
                moderatorUserId = broadcasterUserId;
            }
            _subscriptionBuilder.CreateAllSubscriptionRequests(broadcasterUserId, moderatorUserId);
            var subscriptions = _subscriptionBuilder.GetEventSubSubscribeRequestsWithSessionId(sessionId);

            foreach (var subscription in subscriptions)
            {
                await CreateSubscriptionAsync(subscription);
            }
        }

        private async UniTask CreateSubscriptionAsync(EventSubSubscribeRequest subscription)
        {
            var json = subscription.ToJson();
            Debug.Log("[TwitchEventSubApiClient] Json:" + json);

            var url = DebugMode == true ? API_DEBUG_URL : API_URL;
            var unityWebRequest = CreateUnityWebRequest(url, UnityWebRequest.kHttpVerbPOST, json);
            using (unityWebRequest)
            {
                var result = await unityWebRequest.SendWebRequest().ToUniTask();
            }
        }

        public async UniTask<EventSubSubscriptionData> GetEventSubSubscriptionsAsync()
        {
            var url = DebugMode == true ? API_DEBUG_URL : API_URL;
            url = url + "?status=enabled";
            var unityWebRequest = CreateUnityWebRequest(url, UnityWebRequest.kHttpVerbGET);

            EventSubSubscriptionData eventSubSubscriptionData = null;
            using (unityWebRequest)
            {
                var result = await unityWebRequest.SendWebRequest().ToUniTask();
                Debug.Log(result.downloadHandler.text);
                eventSubSubscriptionData = JsonWrapper.ConvertFromJson<subscription_data>(result.downloadHandler.text).ConvertRawToModel();
            }

            return eventSubSubscriptionData;
        }

        public async UniTask DeleteEventSubSubscriptionsAsync(EventSubSubscriptionData subscriptions)
        {
            var url = DebugMode == true ? API_DEBUG_URL : API_URL;
            List<UnityWebRequest> unityWebRequests = new List<UnityWebRequest>();

            foreach(var subscription in subscriptions.Subscriptions) 
            {
                unityWebRequests.Add(CreateUnityWebRequest(url + $"?id={subscription.Id}", UnityWebRequest.kHttpVerbDELETE));
            }

            await unityWebRequests.Select(x => x.SendWebRequest().ToUniTask());
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