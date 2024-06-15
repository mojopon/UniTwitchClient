using System;

namespace UniTwitchClient.Chat
{
    public interface ITwitchIrcClient : IDisposable
    {
        IObservable<string> OnMessageAsObservable { get; }

        void Connect(string channelName);
        void Close();

        void SendMessage(string message);
    }
}
