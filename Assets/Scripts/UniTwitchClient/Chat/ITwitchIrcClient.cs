using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
