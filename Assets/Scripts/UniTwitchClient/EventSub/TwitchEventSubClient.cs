using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using UniTwitchClient.EventSub.Api;
using UniTwitchClient.EventSub.WebSocket;
using UnityEngine.Playables;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace UniTwitchClient.EventSub
{
    public class TwitchEventSubClient : IDisposable
    {
        public IObservable<ChannelFollow> OnChannelFollowAsObservable { get; private set; }
        public IObservable<ChannelSubscribe> OnChannelSubscribeAsObservable { get; private set; }
        public IObservable<ChannelPointsCustomRewardRedemptionAdd> OnChannelPointsCustomRewardRedemptionAddAsObservable { get; private set; }
        public IObservable<Exception> OnErrorAsObservable { get; private set; }

        private CompositeDisposable _disposables = new CompositeDisposable();

        private ITwitchEventSubWebsocketClient _wsClient;
        private ITwitchEventSubApiClient _apiClient;

        private string _broadcasterUserId;
        private string _sessionId;
        private int timeoutSeconds = 10;
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        private Subject<ChannelFollow> _onChannelFollowSubject;
        private Subject<ChannelSubscribe> _onChannelSubscribeSubject;
        private Subject<ChannelPointsCustomRewardRedemptionAdd> _onChannelPointsCustomRewardRedemptionAddSubject;
        private Subject<Exception> _onErrorSubject;

        private Dictionary<SubscriptionType, INotificationHandler> _handlerDict = new Dictionary<SubscriptionType, INotificationHandler>();

        public TwitchEventSubClient(ConnectionCredentials connectionCredentials) 
        {
            var wsClient = new TwitchEventSubWebsocketClient();
            var apiClient = new TwitchEventSubApiClient(connectionCredentials.ToApiCredentials());

            Initialize(wsClient, apiClient);
        }

        public TwitchEventSubClient(ITwitchEventSubWebsocketClient wsClient, ITwitchEventSubApiClient apiClient) 
        {
            Initialize(wsClient, apiClient);
        }

        private void Initialize(ITwitchEventSubWebsocketClient wsClient, ITwitchEventSubApiClient apiClient) 
        {
            InitializeHandlers();
            InitializeObservables();
            InitializeWebSocketClient(wsClient);
            InitializeApiClient(apiClient);
        }

        public async UniTask ConnectChannelAsync(string broadcasterUserId) 
        {
            if (!string.IsNullOrEmpty(_broadcasterUserId)) { return; }

            _broadcasterUserId = broadcasterUserId;
            var getWelcomeMessageTask = _wsClient.OnWelcomeMessageAsObservable.ToUniTask(useFirstValue: true, cancellationToken: _cancellationTokenSource.Token);

            _wsClient.Connect();

            Welcome welcomeMessage = null;
            try 
            {
                welcomeMessage = await getWelcomeMessageTask.Timeout(TimeSpan.FromSeconds(timeoutSeconds));
                if (welcomeMessage != null) 
                {
                    _sessionId = welcomeMessage.SessionId;
                    await _apiClient.CreateEventSubSubscriptionsAsync(_broadcasterUserId, _sessionId).Timeout(TimeSpan.FromSeconds(timeoutSeconds));
                }
            }
            catch (Exception ex)
            {
                _wsClient.Disconnect();
                throw new Exception("connection failure.");
            }
        }

        public async UniTask DisconnectChannelAsync() 
        {
            await _apiClient.DeleteEventSubSubscriptionsAsync(_sessionId);

            _broadcasterUserId = null;
            _sessionId = null;
            _wsClient.Disconnect();
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }

        private void InitializeWebSocketClient(ITwitchEventSubWebsocketClient wsClient)
        {
            _wsClient = wsClient;

            _wsClient.OnKeepAliveAsObservable.Subscribe(x =>
            {
                Debug.Log("[Example] Keepalive");
            }).AddTo(_disposables);

            _wsClient.OnNotificationAsObservable.Subscribe(x =>
            {
                Debug.Log("[Example] Notification");
                HandleNotification(x);
            }).AddTo(_disposables);

            _wsClient.OnErrorAsObservable.Subscribe(x => HandleError(x)).AddTo(_disposables);

            _wsClient.AddTo(_disposables);
        }

        private void InitializeApiClient(ITwitchEventSubApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        private void InitializeHandlers()
        {
            _handlerDict.Add(SubscriptionType.ChannelFollow, new ChannelFollowHandler(OnChannelFollow));
            _handlerDict.Add(SubscriptionType.ChannelSubscribe, new ChannelSubscribeHandler(OnChannelSubscribe));
            _handlerDict.Add(SubscriptionType.ChannelPointsCustomRewardRedemptionAdd, new ChannelPointsCustomRewardRedemptionAddHandler(OnChannelPointsCustomRewardRedemptionAdd));
        }

        private void InitializeObservables()
        {
            _onChannelFollowSubject = new Subject<ChannelFollow>().AddTo(_disposables);
            _onChannelSubscribeSubject = new Subject<ChannelSubscribe>().AddTo(_disposables);
            _onChannelPointsCustomRewardRedemptionAddSubject = new Subject<ChannelPointsCustomRewardRedemptionAdd>().AddTo(_disposables);
            _onErrorSubject = new Subject<Exception>().AddTo(_disposables);

            OnChannelFollowAsObservable = _onChannelFollowSubject.AsObservable();
            OnChannelSubscribeAsObservable = _onChannelSubscribeSubject.AsObservable();
            OnChannelPointsCustomRewardRedemptionAddAsObservable = _onChannelPointsCustomRewardRedemptionAddSubject.AsObservable();
            OnErrorAsObservable = _onErrorSubject.AsObservable();
        }

        private void HandleNotification(WebSocket.Notification notification)
        {
            if (_handlerDict.ContainsKey(notification.SubscriptionType))
            {
                _handlerDict[notification.SubscriptionType].HandleNotification(notification);
            }
            else 
            {
                Debug.LogWarning("No correspoinding Notification Handler exists. SubscriptionType:" + notification.SubscriptionType);
            }
        }

        private void HandleError(Exception exception) 
        {
            _cancellationTokenSource.Cancel();
            _onErrorSubject.OnNext(exception);
        }

        private void OnChannelFollow(ChannelFollow channelFollow)
        {
            _onChannelFollowSubject.OnNext(channelFollow);
        }

        private void OnChannelSubscribe(ChannelSubscribe channelSubscribe) 
        {
            _onChannelSubscribeSubject.OnNext(channelSubscribe);
        }

        private void OnChannelPointsCustomRewardRedemptionAdd(ChannelPointsCustomRewardRedemptionAdd channelPointsCustomRewardRedemptionAdd) 
        {
            _onChannelPointsCustomRewardRedemptionAddSubject.OnNext(channelPointsCustomRewardRedemptionAdd);
        }
    }
}