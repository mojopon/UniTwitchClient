using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using UniTwitchClient.EventSub.Api;

namespace UniTwitchClient.EventSub
{
    public class TwitchEventSubClient : IDisposable, ISubscriptionManager
    {
        public IObservable<ChannelFollow> OnChannelFollowAsObservable { get; private set; }
        public IObservable<ChannelSubscribe> OnChannelSubscribeAsObservable { get; private set; }

        private CompositeDisposable _disposables = new CompositeDisposable();
        private ITwitchEventSubWebsocketClient _wsClient;
        private ITwitchEventSubApiClient _apiClient;

        private Action<ChannelFollow> _onChannelFollowReceived;
        private Action<ChannelSubscribe> _onChannelSubscribeReceived;

        private Dictionary<SubscriptionType, INotificationHandler> _handlerDict = new Dictionary<SubscriptionType, INotificationHandler>();

        public TwitchEventSubClient(ConnectionCredentials connectionCredentials) : this(connectionCredentials, null, null) { }

        public TwitchEventSubClient(ConnectionCredentials connectionCredentials, ITwitchEventSubWebsocketClient wsClient, ITwitchEventSubApiClient apiClient) 
        {
            if (wsClient == null) 
            {
                _wsClient = new TwitchEventSubWebsocketClient();
            }
            InitializeWebSocketClient(wsClient);

            if (apiClient == null)
            {
                apiClient = new TwitchEventSubApiClient(connectionCredentials.ToApiCredentials());
            }
            InitializeApiClient(apiClient);


            InitializeHandlers();
            InitializeObservables();
        }

        public void Connect()
        {
            _wsClient.Connect();
        }

        public void Dispose()
        {
            _wsClient.Dispose();
            _disposables.Dispose();
        }

        private void InitializeWebSocketClient(ITwitchEventSubWebsocketClient wsClient)
        {
            _wsClient = wsClient;


            _wsClient.OnWelcomeMessageAsObservable.Subscribe(x =>
            {
                Debug.Log("[Example] Welcome");
                _apiClient.CreateSubscriptionsAsync(x.SessionId);
            }).AddTo(_disposables);

            _wsClient.OnKeepAliveAsObservable.Subscribe(x =>
            {
                Debug.Log("[Example] Keepalive");
            }).AddTo(_disposables);

            _wsClient.OnNotificationAsObservable.Subscribe(x =>
            {
                Debug.Log("[Example] Notification");
                HandleNotification(x);
            }).AddTo(_disposables);
        }

        private void InitializeApiClient(ITwitchEventSubApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        private void InitializeHandlers()
        {
            _handlerDict.Add(SubscriptionType.ChannelFollow, new ChannelFollowHandler(OnChannelFollow));
            _handlerDict.Add(SubscriptionType.ChannelSubscribe, new ChannelSubscribeHandler(OnChannelSubscribe));
        }

        private void InitializeObservables()
        {
            OnChannelFollowAsObservable = Observable.FromEvent<ChannelFollow>(
                                        h => _onChannelFollowReceived += h,
                                        h => _onChannelFollowReceived -= h)
                                        .ObserveOnMainThread()
                                        .Share();

            OnChannelSubscribeAsObservable = Observable.FromEvent<ChannelSubscribe>(
                                        h => _onChannelSubscribeReceived += h,
                                        h => _onChannelSubscribeReceived -= h)
                                        .ObserveOnMainThread()
                                        .Share();
        }

        private void HandleNotification(WebSocket.Notification notification)
        {
            _handlerDict[notification.SubscriptionType].HandleNotification(notification);
        }

        private void OnChannelFollow(ChannelFollow channelFollow)
        {
            _onChannelFollowReceived?.Invoke(channelFollow);
        }

        private void OnChannelSubscribe(ChannelSubscribe channelSubscribe) 
        {
            _onChannelSubscribeReceived?.Invoke(channelSubscribe);
        }

        #region ISubscriptionManager

        public void SubscribeChannelFollow(string broadcasterUserId, string moderatorUserId = null)
        {
            _apiClient.SubscribeChannelFollow(broadcasterUserId, moderatorUserId);
        }

        public void SubscribeChannelSubscribe(string broadcasterUserId)
        {
            _apiClient.SubscribeChannelSubscribe(broadcasterUserId);
        }

        public void SubscribeChannelPointsCustomRewardRedemptionAdd(string broadcasterUserId)
        {
            _apiClient.SubscribeChannelPointsCustomRewardRedemptionAdd(broadcasterUserId);
        }

        #endregion
    }
}