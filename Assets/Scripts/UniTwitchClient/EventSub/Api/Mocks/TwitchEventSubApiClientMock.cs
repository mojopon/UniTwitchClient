using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UniTwitchClient.EventSub.Api.Models;
using UnityEngine;

namespace UniTwitchClient.EventSub.Api.Mocks
{
    public class TwitchEventSubApiClientMock : ITwitchEventSubApiClient
    {
        public List<TwitchEventSubApiCalledMethodLog> CalledMethods { get; private set; } = new List<TwitchEventSubApiCalledMethodLog>();

        public async UniTask CreateEventSubSubscriptionsAsync(string broadcasterUserId, string sessionId)
        {
            await CreateEventSubSubscriptionsAsync(broadcasterUserId, sessionId, broadcasterUserId);
        }

        public async UniTask CreateEventSubSubscriptionsAsync(string broadcasterUserId, string sessionId, string moderatorUserId)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("broadcasterUserId", broadcasterUserId);
            parameters.Add("sessionId", sessionId);
            parameters.Add("moderatorUserId", moderatorUserId);

            CalledMethods.Add(new TwitchEventSubApiCalledMethodLog(TwitchEventSubApiCalledMethodLog.MethodType.Create, parameters));

            await UniTask.CompletedTask;
        }

        public UniTask DeleteEventSubSubscriptionsAsync(EventSubSubscriptionData subscriptions)
        {
            throw new System.NotImplementedException();
        }

        public async UniTask DeleteEventSubSubscriptionsAsync(string sessionId)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("sessionId", sessionId);
            //CalledMethods.Add(new TwitchEventSubApiCalledMethodLog(TwitchEventSubApiCalledMethodLog.MethodType.Delete, parameters));

            await UniTask.CompletedTask;
        }

        public UniTask<EventSubSubscriptionData> GetEventSubSubscriptionsAsync()
        {
            throw new System.NotImplementedException();
        }
    }

    public class TwitchEventSubApiCalledMethodLog 
    {
        public enum MethodType 
        {
            None,
            Create,
            Delete,
            Get,
        }

        public MethodType Type { get; private set; }
        public Dictionary<string, string> Parameters { get; private set; }

        public TwitchEventSubApiCalledMethodLog(MethodType methodType, Dictionary<string,string> parameters) 
        {
            Type = methodType;
            Parameters = parameters;
        }
    }
}
