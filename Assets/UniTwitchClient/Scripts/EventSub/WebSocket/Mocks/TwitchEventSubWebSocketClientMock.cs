using System;
using UniRx;
using UniTwitchClient.Common;
using UniTwitchClient.EventSub.WebSocket;
using Notification = UniTwitchClient.EventSub.WebSocket.Notification;

namespace UniTwitchClient.EventSub.Mocks
{
    public class TwitchEventSubWebSocketClientMock : ITwitchEventSubWebsocketClient
    {
        public IUniTwitchLogger Logger { get; set; } = new UniTwitchProductionLogger();

        public IObservable<Welcome> OnWelcomeMessageAsObservable { get; private set; }
        public IObservable<KeepAlive> OnKeepAliveAsObservable { get; private set; }
        public IObservable<Notification> OnNotificationAsObservable { get; private set; }
        public IObservable<Exception> OnErrorAsObservable { get; private set; }

        public bool IsConnected { get; private set; }
        public bool IsDisposed { get; private set; }

        private Subject<Welcome> _welcomeSubject = new Subject<Welcome>();
        private Subject<KeepAlive> _keepAliveSubject = new Subject<KeepAlive>();
        private Subject<Notification> _notificationSubject = new Subject<Notification>();
        private Subject<Exception> _errorSubject = new Subject<Exception>();

        private CompositeDisposable disposables = new CompositeDisposable();

        public TwitchEventSubWebSocketClientMock()
        {
            disposables.Add(_welcomeSubject);
            disposables.Add(_keepAliveSubject);
            disposables.Add(_notificationSubject);
            disposables.Add(_errorSubject);

            OnWelcomeMessageAsObservable = _welcomeSubject.AsObservable();
            OnKeepAliveAsObservable = _keepAliveSubject.AsObservable();
            OnNotificationAsObservable = _notificationSubject.AsObservable();
            OnErrorAsObservable = _errorSubject.AsObservable();
        }

        public void Connect()
        {
            IsConnected = true;
        }

        public void Disconnect()
        {
            IsConnected = false;
        }

        public void Dispose()
        {
            disposables.Dispose();
            IsDisposed = true;
        }

        public void ReceiveWelcomeMessage(Welcome welcomeMessage)
        {
            _welcomeSubject.OnNext(welcomeMessage);
        }

        public void ReceiveNotification(Notification notification)
        {
            _notificationSubject.OnNext(notification);
        }

        public void ReceiveError(Exception error)
        {
            _errorSubject.OnNext(error);
        }
    }
}