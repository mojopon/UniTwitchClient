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
    public class TwitchEventSubApiClient : ISubscriptionManager
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

        private SubscriptionBuilder _subscriptionBuilder;
        private ApiCredentials _apiCredentials;
        private bool _debugMode;

        public TwitchEventSubApiClient(ApiCredentials apiCredentials)
        {
            _subscriptionBuilder = new SubscriptionBuilder();
            _apiCredentials = apiCredentials;
        }

        public void CreateSubscriptions(string sessionId)
        {
            _ = CreateSubscriptionsAsync(sessionId);
        }

        private async UniTask CreateSubscriptionsAsync(string sessionId)
        {
            var subscriptions = _subscriptionBuilder.GetSubscriptionsWithSessionId(sessionId);

            foreach (var subscription in subscriptions)
            {
                await CreateSubscriptionAsync(subscription);
            }
        }

        private async UniTask CreateSubscriptionAsync(Subscription subscription)
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

            Debug.Log("created subscription.");
        }

        public async UniTask CreateEventSubSucscriptionAsync(string sessionId, SubscriptionType subscriptionType)
        {
            var request = new SubscribeRequest()
            {
                SubscriptionType = subscriptionType,
                ConditionBroadCasterUserId = "1234",
                ConditionModeraterUserId = "1234",
                TransportSessionId = sessionId,
            };
            var json = request.ToJson();
            var postData = Encoding.UTF8.GetBytes(json);

            Debug.Log(json);

            //await UniTask.SwitchToMainThread();

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
            await uwr.SendWebRequest().ToUniTask();
        }

        public void SubscribeChannelFollow(string broadcasterUserId, string moderatorUserId = null)
        {
            _subscriptionBuilder.SubscribeChannelFollow(broadcasterUserId, moderatorUserId);
        }

        public void SubscribeChannelSubscribe(string broadcasterUserId)
        {
            _subscriptionBuilder.SubscribeChannelSubscribe(broadcasterUserId);
        }
    }
}