using UnityEngine;
using UniRx;
using System;
using UniTwitchClient.EventSub.Api;
using UniTwitchClient.EventSub.WebSocket;
using Cysharp.Threading.Tasks;
using System.Threading;
using UniTwitchClient.EventSub.Converters;

namespace UniTwitchClient.EventSub
{
    public class TwitchEventSubClient : IDisposable
    {
        public IObservable<ChannelFollow> OnChannelFollowAsObservable { get; private set; }
        public IObservable<ChannelSubscribe> OnChannelSubscribeAsObservable { get; private set; }
        public IObservable<ChannelSubscriptionMessage> OnChannelSubscriptionMessageAsObservable { get; private set; }
        public IObservable<ChannelPointsCustomRewardRedemptionAdd> OnChannelPointsCustomRewardRedemptionAddAsObservable { get; private set; }
        public IObservable<Exception> OnErrorAsObservable { get; private set; }

        private CompositeDisposable _disposables = new CompositeDisposable();

        private ITwitchEventSubWebsocketClient _wsClient;
        private ITwitchEventSubApiClient _apiClient;

        private string _broadcasterUserId;
        private string _sessionId;
        private int timeoutSeconds = 10;
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        private SubjectComposition _subjectComposition = new SubjectComposition();
        private NotificationConverter _notificationConverter = new NotificationConverter();

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
                throw new Exception("an error has occured while connecting. error:" + ex);
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

        private void InitializeObservables()
        {
            OnChannelFollowAsObservable = _subjectComposition.CreateSubject<ChannelFollow>(SubscriptionType.ChannelFollow.ToString()).AsObservable();
            OnChannelSubscribeAsObservable = _subjectComposition.CreateSubject<ChannelSubscribe>(SubscriptionType.ChannelSubscribe.ToString()).AsObservable();
            OnChannelSubscriptionMessageAsObservable = _subjectComposition.CreateSubject<ChannelSubscriptionMessage>(SubscriptionType.ChannelSubscriptionMessage.ToString()).AsObservable();
            OnChannelPointsCustomRewardRedemptionAddAsObservable = _subjectComposition.CreateSubject<ChannelPointsCustomRewardRedemptionAdd>(SubscriptionType.ChannelPointsCustomRewardRedemptionAdd.ToString()).AsObservable();
            OnErrorAsObservable = _subjectComposition.CreateSubject<Exception>("Exception").AsObservable();

            _subjectComposition.AddTo(_disposables);
        }

        private void HandleNotification(WebSocket.Notification notification)
        {
            _subjectComposition.OnNext(notification.SubscriptionType.ToString(), _notificationConverter.Convert(notification));
        }

        private void HandleError(Exception exception) 
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();
            _subjectComposition.OnNext("Exception", exception);
        }
    }
}