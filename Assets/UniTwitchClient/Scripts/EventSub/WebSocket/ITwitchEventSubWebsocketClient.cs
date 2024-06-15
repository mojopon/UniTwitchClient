using System;
using UniTwitchClient.Common;

namespace UniTwitchClient.EventSub.WebSocket
{
    public interface ITwitchEventSubWebsocketClient : IDisposable
    {
        IUniTwitchLogger Logger { get; set; }

        void Connect();
        void Disconnect();

        IObservable<Welcome> OnWelcomeMessageAsObservable { get; }
        IObservable<KeepAlive> OnKeepAliveAsObservable { get; }
        IObservable<Notification> OnNotificationAsObservable { get; }
        IObservable<Exception> OnErrorAsObservable { get; }
    }
}