using Cysharp.Threading.Tasks;
using System;
using UniRx;
using UniTwitchClient.Chat.Models;

namespace UniTwitchClient.Chat
{
    public class TwitchChatClient : IDisposable
    {
        public IObservable<TwitchChatMessage> TwitchChatMessageAsObservable => _onTwitchChatMessageSubject.AsObservable();
        private Subject<TwitchChatMessage> _onTwitchChatMessageSubject = new Subject<TwitchChatMessage>();

        public IObservable<string> MessageRawAsObservable => _onMessageRawSubject.AsObservable();
        private Subject<string> _onMessageRawSubject = new Subject<string>();

        public ConnectionState State
        {
            get { return _state; }
            private set
            {
                _state = value;
            }
        }
        private ConnectionState _state = ConnectionState.Idle;

        private ITwitchIrcClient _client;
        private TwitchIrcCredentials _ircCredentials;

        private CompositeDisposable _disposables = new CompositeDisposable();

        public TwitchChatClient(TwitchIrcCredentials credentials) : this(credentials, null) { }

        public TwitchChatClient(TwitchIrcCredentials credentials, ITwitchIrcClient client = null)
        {
            _ircCredentials = credentials;

            _disposables.Add(_onTwitchChatMessageSubject);
            _disposables.Add(_onMessageRawSubject);

            InitializeIrcClient(client);
        }

        public void Connect(string channelName)
        {
            State = ConnectionState.Connecting;
            _client.Connect(channelName);
        }

        public void Close()
        {
            _client.Close();
            State = ConnectionState.Disconnected;
        }

        private void InitializeIrcClient(ITwitchIrcClient ircClient = null)
        {
            if (ircClient == null)
            {
                ircClient = new TwitchIrcClient(_ircCredentials);
            }

            _client = ircClient;
            _disposables.Add(_client);
            _client.OnMessageAsObservable.Subscribe(x => HandleMessage(x), ex => HandleError(ex), () => HandleComplete()).AddTo(_disposables);
        }

        public void Dispose()
        {
            _disposables?.Dispose();
        }

        private void HandleMessage(string messageRaw)
        {
            _onMessageRawSubject.OnNext(messageRaw);

            TwitchChatMessage message = null;
            try
            {
                message = IrcMessageParser.ParseMessage(messageRaw);
            }
            catch (Exception ex)
            {
                HandleError(ex);
                return;
            }

            if (message.Command == TwitchIrcCommand.Numeric001)
            {
                HandleLoginSucceeded();
            }

            if (message != null)
            {
                _onTwitchChatMessageSubject.OnNext(message);
            }
        }

        private void HandleLoginSucceeded()
        {
            State = ConnectionState.Connected;
        }

        private void HandleError(Exception ex)
        {
            _onTwitchChatMessageSubject.OnError(ex);
        }

        private void HandleComplete()
        {

        }
    }
}