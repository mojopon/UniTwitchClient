using Cysharp.Threading.Tasks;
using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using UniRx;

namespace UniTwitchClient.Chat
{
    public class TwitchIrcClient : ITwitchIrcClient
    {
        private const string TWITCH_IRC_URL = "irc.chat.twitch.tv";
        private const int TWITCH_IRC_PORT = 6667;

        private TcpClient _tcpClient = new TcpClient();
        private StreamReader _reader;
        private StreamWriter _writer;

        private bool _connecting;
        private bool _connected => _tcpClient != null && _tcpClient.Connected;

        public IObservable<string> OnMessageAsObservable => _messageSubject.AsObservable();
        private Subject<string> _messageSubject = new Subject<string>();

        private CancellationTokenSource _cts;
        private CancellationToken _ct;

        private TwitchIrcCredentials _credentials;
        public TwitchIrcClient(TwitchIrcCredentials credentials)
        {
            _credentials = credentials;
        }

        public void Connect(string channelName)
        {
            _cts = new CancellationTokenSource();
            _ct = _cts.Token;

            UniTask.Create(async () =>
            {
                await ConnectAsync(channelName);
            })
            .AttachExternalCancellation(_ct);
        }

        public void Close()
        {
            _cts.Cancel();

            _tcpClient.Close();

            _reader?.Dispose();
            _reader = null;

            _writer?.Dispose();
            _writer = null;

            _receiveNullMessageCount = 0;
        }

        public async UniTask ConnectAsync(string channelName)
        {
            if (_connecting || _connected)
            {
                return;
            }

            _connecting = true;

            try
            {
                await _tcpClient.ConnectAsync(TWITCH_IRC_URL, TWITCH_IRC_PORT);
            }
            catch (Exception ex)
            {
                _messageSubject.OnError(ex);
                return;
            }


            if (_tcpClient.Connected)
            {
                _connecting = false;
                _reader = new StreamReader(_tcpClient.GetStream());
                _writer = new StreamWriter(_tcpClient.GetStream());
            }

            if (!string.IsNullOrEmpty(_credentials.TwitchOAuthToken))
                await _writer.WriteLineAsync($"PASS {_credentials.TwitchOAuthToken}");

            if (!string.IsNullOrEmpty(_credentials.TwitchUsername))
                await _writer.WriteLineAsync($"NICK {_credentials.TwitchUsername}");

            await _writer.WriteLineAsync($"JOIN #{channelName}");

            await _writer.WriteLineAsync("CAP REQ :twitch.tv/tags");
            await _writer.WriteLineAsync("CAP REQ :twitch.tv/commands");
            await _writer.WriteLineAsync("CAP REQ :twitch.tv/membership");

            await _writer.FlushAsync();


            UniTask.Create(async () =>
            {
                await ListenAsync();
            })
            .AttachExternalCancellation(_ct)
            .Forget();
        }

        private int _receiveNullMessageCount = 0;
        private async UniTask ListenAsync()
        {
            while (_tcpClient.Connected)
            {
                try
                {
                    var message = await _reader.ReadLineAsync();
                    if (message != null)
                    {
                        HandleReceivedMessage(message);
                    }
                    else
                    {
                        _receiveNullMessageCount++;
                    }

                    if (_receiveNullMessageCount > 10)
                    {
                        Close();
                        HandleError(new Exception("An error occurred in the TwitchIrcClient connection."));
                        return;
                    }

                }
                catch (Exception ex)
                {
                    HandleError(ex);
                    return;
                }

                await UniTask.Yield();
            }
        }

        private void HandleReceivedMessage(string message)
        {
            _messageSubject.OnNext(message);

            if (message.Contains("PING :tmi.twitch.tv"))
            {
                HandlePing();
            }

            if (message.Contains(":tmi.twitch.tv NOTICE * :Login authentication failed"))
            {
                HandleError(new Exception("Login authentication failed."));
            }

            if (message.Contains(":tmi.twitch.tv NOTICE * :Improperly formatted auth"))
            {
                HandleError(new Exception("Improperly formatted auth"));
            }
        }

        private void HandlePing()
        {
            SendMessage("PONG :tmi.twitch.tv");
        }

        private void HandleError(Exception ex)
        {
            _messageSubject.OnError(ex);
            _messageSubject.Dispose();
            _messageSubject = new Subject<string>();
        }

        public void SendMessage(string message)
        {
            if (_writer != null)
            {
                UniTask.Create(async () =>
                {
                    await SendMessageAsync(message);
                })
                .AttachExternalCancellation(_ct);
            }
        }

        private async UniTask SendMessageAsync(string message)
        {
            await _writer.WriteLineAsync(message);
            await _writer.FlushAsync();
        }

        public void Dispose()
        {
            _cts.Cancel();
            _cts = null;

            _reader?.Dispose();
            _writer?.Dispose();

            _tcpClient.Close();
            _tcpClient.Dispose();

            _messageSubject.OnCompleted();
            _messageSubject.Dispose();
        }
    }
}
