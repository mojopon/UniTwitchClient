using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace UniTwitchClient.Chat.Mocks
{
    public class TwitchIrcClientMock : ITwitchIrcClient
    {
        public IObservable<string> OnMessageAsObservable => _messageSubject.AsObservable();

        private Subject<string> _messageSubject = new Subject<string>();

        public void Close()
        {

        }

        public void Connect(string channelName)
        {
        }

        public void Dispose()
        {
            _messageSubject.Dispose();
        }

        public void SendMessage(string message)
        {
            throw new NotImplementedException();
        }

        public void ReceiveMessage(string message) 
        {
            _messageSubject.OnNext(message);
        }
    }
}
