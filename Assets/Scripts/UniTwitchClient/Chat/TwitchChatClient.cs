using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using UniRx;
using UniTwitchClient.Chat.Models;
using UnityEngine;
using UnityEngine.Networking.PlayerConnection;

namespace UniTwitchClient.Chat
{
    public class TwitchChatClient : IDisposable
    {
        private ITwitchIrcClient _client;
        private IrcCredentials _ircCredentials;

        private CompositeDisposable _disposables;

        private Subject<TwitchChatMessage> _onTwitchChatMessageSubject = new Subject<TwitchChatMessage>();
        private Subject<string> _onMessageRawSubject = new Subject<string>();

        public TwitchChatClient(ConnectionCredentials credentials)
        {
            _ircCredentials = credentials.ToIrcCredentials();
            InitializeIrcClient();
        }

        public void Connect(string channelName) 
        {
            _client.Connect(channelName);
        }

        private void InitializeIrcClient() 
        {
            if (_client != null) 
            {
                _disposables.Dispose();
                _client = null;
            }

            _disposables = new CompositeDisposable();
            _client = new TwitchIrcClient(_ircCredentials);
            _disposables.Add(_client);
            _client.OnMessageAsObservable.Subscribe(x => HandleMessage(x),ex => HandleError(ex),() => HandleComplete()).AddTo(_disposables);
        }

        public void Dispose()
        {
            _disposables?.Dispose();
        }

        private void HandleMessage(string message) 
        {
            Debug.Log($"[TwitchChatClient] message:{message}");
            _onMessageRawSubject.OnNext(message);

        }

        private void HandleError(Exception ex) 
        {
            Debug.Log($"[TwitchChatClient] an error occured:{ex.Message}");

        }

        private void HandleComplete()
        {

        }
    }
}