using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;
using System.Text;
using System.Threading;
using UniTwitchClient.EventSub.Api.Models;

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

            var postData = Encoding.UTF8.GetBytes(json);

            var url = DebugMode == true ? API_DEBUG_URL : API_URL;
            Debug.Log("[TwitchEventSubApiClient] Post to " + url);

            using var uwr = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST)
            {
                uploadHandler = new UploadHandlerRaw(postData),
                downloadHandler = new DownloadHandlerBuffer()
            };
            uwr.SetRequestHeader("Authorization", _apiCredentials.AuthorizationBearer);
            uwr.SetRequestHeader("Client-Id", _apiCredentials.ClientId);
            uwr.SetRequestHeader("Content-type", "application/json");

            var result = await uwr.SendWebRequest().ToUniTask();

            //await GetEventSubSubscriptionsAsync();
            Debug.Log("created subscription.");
        }

        public async UniTask<List<EventSubSubscription>> GetEventSubSubscriptionsAsync()
        {
            var url = DebugMode == true ? API_DEBUG_URL : API_URL;
            using var uwr = new UnityWebRequest(url, UnityWebRequest.kHttpVerbGET)
            {
                downloadHandler = new DownloadHandlerBuffer()
            };
            uwr.SetRequestHeader("Authorization", _apiCredentials.AuthorizationBearer);
            uwr.SetRequestHeader("Client-Id", _apiCredentials.ClientId);

            var result = await uwr.SendWebRequest().ToUniTask();
            Debug.Log("GetEventSubSubscriptionsAsync");
            return null;
        }

        public UniTask DeleteEventSubSubscriptionsAsync(string sessionId)
        {
            throw new System.NotImplementedException();
        }
    }
}