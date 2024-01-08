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
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Networking.PlayerConnection;

namespace UniTwitchClient.Chat
{
    public class TwitchChatClient : IDisposable
    {
        public IObservable<TwitchChatMessage> TwitchChatMessageAsObservable => _onTwitchChatMessageSubject.AsObservable();
        public IObservable<string> MessageRawAsObservable => _onMessageRawSubject.AsObservable();

        private ITwitchIrcClient _client;
        private IrcCredentials _ircCredentials;

        private CompositeDisposable _disposables = new CompositeDisposable();

        private Subject<TwitchChatMessage> _onTwitchChatMessageSubject = new Subject<TwitchChatMessage>();
        private Subject<string> _onMessageRawSubject = new Subject<string>();

        public TwitchChatClient(ConnectionCredentials credentials) : this(credentials, null) { }

        public TwitchChatClient(ConnectionCredentials credentials, ITwitchIrcClient client = null)
        {
            _ircCredentials = credentials.ToIrcCredentials();

            _disposables.Add(_onTwitchChatMessageSubject);
            _disposables.Add(_onMessageRawSubject);

            InitializeIrcClient(client);
        }

        public void Connect(string channelName) 
        {
            _client.Connect(channelName);
        }

        private void InitializeIrcClient(ITwitchIrcClient ircClient = null) 
        {
            if (ircClient == null) 
            {
                ircClient = new TwitchIrcClient(_ircCredentials);
            }

            _client = ircClient;
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