using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniTwitchClient.EventSub.WebSocket;
using Notification = UniTwitchClient.EventSub.WebSocket.Notification;
using UnityEngine;

public class TwitchEventSubWebSocketClientMock : ITwitchEventSubWebsocketClient
{
    public IObservable<Welcome> OnWelcomeMessageAsObservable { get; private set; }
    public IObservable<KeepAlive> OnKeepAliveAsObservable { get; private set; }
    public IObservable<Notification> OnNotificationAsObservable { get; private set; }
    public IObservable<Exception> OnErrorAsObservable { get; private set; }

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

        OnWelcomeMessageAsObservable = _welcomeSubject.AsObservable().ObserveOnMainThread().Share();
        OnKeepAliveAsObservable = _keepAliveSubject.AsObservable().ObserveOnMainThread().Share();
        OnNotificationAsObservable = _notificationSubject.AsObservable().ObserveOnMainThread().Share();
        OnErrorAsObservable = _errorSubject.AsObservable().ObserveOnMainThread().Share();
    }

    public void Connect()
    {
    }

    public void Disconnect()
    {
    }

    public void Dispose()
    {
        disposables.Dispose();
    }
}
