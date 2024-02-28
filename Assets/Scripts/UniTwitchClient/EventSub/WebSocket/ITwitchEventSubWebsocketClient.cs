using System;
using System.Collections;
using System.Collections.Generic;
using UniTwitchClient.EventSub.WebSocket;
using UnityEngine;

public interface ITwitchEventSubWebsocketClient : IDisposable
{
    void Connect();
    void Disconnect();

    IObservable<Welcome> OnWelcomeMessageAsObservable { get; }
    IObservable<KeepAlive> OnKeepAliveAsObservable { get; }
    IObservable<Notification> OnNotificationAsObservable { get; }
    IObservable<Exception> OnErrorAsObservable { get; }
}
