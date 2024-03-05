using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Threading.Tasks;
using WebSocketSharp;

namespace UniTwitchClient.EventSub.WebSocket
{
    public class TwitchEventSubWebsocketClient : ITwitchEventSubWebsocketClient
    {
        public bool DebugMode
        {
            get
            {
                return _debugMode;
            }
            set
            {
                _debugMode = value;
            }
        }

        public IObservable<Welcome> OnWelcomeMessageAsObservable { get; private set; }
        public IObservable<KeepAlive> OnKeepAliveAsObservable { get; private set; }
        public IObservable<Notification> OnNotificationAsObservable { get; private set; }
        public IObservable<Exception> OnErrorAsObservable { get; private set; }

        private event Action<Welcome> _onWelcomeMessageReceived;
        private event Action<KeepAlive> _onKeepAliveMessageReceived;
        private event Action<Notification> _onNotificationReceived;
        private event Action<Exception> _onError;

        private WebSocketSharp.WebSocket _ws;
        private const string WEBSOCKET_DEBUG_URL = "ws://localhost:8080/ws";
        private const string WEBSOCKET_URL = "wss://eventsub.wss.twitch.tv/ws";
        private int _keepAlive_Timeout;
        private bool _disposed;

        private bool _debugMode;

        public TwitchEventSubWebsocketClient()
        {
            Initialize();
        }

        public void Connect()
        {
            var url = DebugMode == true ? WEBSOCKET_DEBUG_URL : WEBSOCKET_URL;
            Debug.Log("[TwitchEventSubWebSocketClient] Connecting to " + url);
            _ws = new WebSocketSharp.WebSocket(url);

            AddHandlers();

            _ws.Connect();
            Debug.Log("Connected");
        }

        public void Disconnect()
        {
            _ws.Close();
        }

        private void Initialize()
        {
            OnWelcomeMessageAsObservable = Observable.FromEvent<Welcome>(
                                               h => _onWelcomeMessageReceived += h,
                                               h => _onWelcomeMessageReceived -= h)
                                               .ObserveOnMainThread()
                                               .Share();

            OnKeepAliveAsObservable = Observable.FromEvent<KeepAlive>(
                                               h => _onKeepAliveMessageReceived += h,
                                               h => _onKeepAliveMessageReceived -= h)
                                               .ObserveOnMainThread()
                                               .Share();

            OnNotificationAsObservable = Observable.FromEvent<Notification>(
                                               h => _onNotificationReceived += h,
                                               h => _onNotificationReceived -= h)
                                               .ObserveOnMainThread()
                                               .Share();
        }

        private void AddHandlers()
        {
            _ws.OnMessage += HandleMessage;
            _ws.OnError += HandleError;
        }

        private void RemoveHandlers()
        {
            _ws.OnMessage -= HandleMessage;
            _ws.OnError -= HandleError;
        }


        private void HandleMessage(object sender, MessageEventArgs e)
        {
            var data = e.Data;
            var messageType = WebSocketMessageConverter.GetMessageType(data);

            Debug.Log(data);
            Debug.Log("MessageType:" + messageType.ToString());

            switch (messageType)
            {
                case WebSocketMessageType.SessionWelcome:
                    {
                        HandleWelcomeMessage(data);
                        break;
                    }
                case WebSocketMessageType.SessionKeepAlive:
                    {
                        HandleKeepaliveMessage(data);
                        break;
                    }
                case WebSocketMessageType.Notification:
                    {
                        HandleNotification(data);
                        break;
                    }
                case WebSocketMessageType.Reconnect:
                    {
                        HandleReconnect(data);
                        break;
                    }
            }
        }

        private void HandleWelcomeMessage(string data)
        {
            var welcomeMessage = WebSocketMessageConverter.ConvertToWelcomeMessage(data);
            _onWelcomeMessageReceived?.Invoke(welcomeMessage);
        }

        private void HandleKeepaliveMessage(string data)
        {
            var keepaliveMessage = WebSocketMessageConverter.ConvertToKeepAliveMessage(data);
            _onKeepAliveMessageReceived?.Invoke(keepaliveMessage);
        }

        private void HandleNotification(string data)
        {
            var notification = WebSocketMessageConverter.ConvertToNotification(data);
            _onNotificationReceived?.Invoke(notification);
        }

        private void HandleReconnect(string data)
        {

        }

        private void HandleError(object sender, ErrorEventArgs e)
        {
            var error = e.Exception;
            _onError?.Invoke(error);
            Debug.LogError(error);
        }

        public void Dispose()
        {
            _ws.Close();

            RemoveHandlers();

            _disposed = true;
        }
    }
}