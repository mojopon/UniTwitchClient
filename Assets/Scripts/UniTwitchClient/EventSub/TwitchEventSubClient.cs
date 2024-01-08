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
        private TwitchEventSubWebsocketClient _wsClient;
        private TwitchEventSubApiClient _apiClient;

        private Action<ChannelFollow> _onChannelFollowReceived;
        private Action<ChannelSubscribe> _onChannelSubscribeReceived;

        private Dictionary<SubscriptionType, INotificationHandler> _handlerDict = new Dictionary<SubscriptionType, INotificationHandler>();

        #region DebugMode
        public bool DebugMode
        {
            get
            {
                return _debugMode;
            }
            set
            {
                _debugMode = value;
                _wsClient.DebugMode = _debugMode;
                _apiClient.DebugMode = _debugMode;
            }
        }
        private bool _debugMode = false;
        #endregion

        public TwitchEventSubClient(ConnectionCredentials connectionCredentials)
        {
            InitializeWebSocketClient();
            InitializeApiClient(connectionCredentials);
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

        private void InitializeWebSocketClient()
        {
            _wsClient = new TwitchEventSubWebsocketClient();

            _wsClient.OnWelcomeMessageAsObservable.Subscribe(x =>
            {
                Debug.Log("[Example] Welcome");
                _apiClient.CreateSubscriptions(x.SessionId);
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

        private void InitializeApiClient(ConnectionCredentials connectionCredentials)
        {
            _apiClient = new TwitchEventSubApiClient(connectionCredentials.ToApiCredentials());
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

        #endregion
    }
}