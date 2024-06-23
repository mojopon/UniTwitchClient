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
            if (_connecting || _connected)
            {
                return;
            }

            _connecting = true;

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
        }

        public async UniTask ConnectAsync(string channelName)
        {
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

        private async UniTask ListenAsync()
        {
            while (_tcpClient.Connected)
            {
                try
                {
                    var message = await _reader.ReadLineAsync();
                    if (!string.IsNullOrEmpty(message))
                    {
                        HandleReceivedMessage(message);
                    }
                    // message‚ªnull‚Ìê‡‚ÍTcpClient‚ªØ’f‚³‚ê‚½ê‡‚È‚Ì‚ÅƒGƒ‰[‚ÅI—¹
                    else
                    {
                        HandleError(new Exception("Twitch IRC Client has been disconnected."));
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
            if (message.Contains("PING :tmi.twitch.tv"))
            {
                HandlePing();
                _messageSubject.OnNext(message);
            }
            else if (message.Contains(":tmi.twitch.tv NOTICE * :Login authentication failed"))
            {
                HandleError(new Exception("Login authentication failed."));
            }
            else if (message.Contains(":tmi.twitch.tv NOTICE * :Improperly formatted auth"))
            {
                HandleError(new Exception("Improperly formatted auth"));
            }
            else 
            {
                _messageSubject.OnNext(message);
            }
        }

        private void HandlePing()
        {
            SendMessage("PONG :tmi.twitch.tv");
        }

        private void HandleError(Exception ex)
        {
            _messageSubject.OnError(ex);
            Close();
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

        public async UniTask SendMessageAsync(string message)
        {
            await _writer.WriteLineAsync(message);
            await _writer.FlushAsync();
        }

        public void Dispose()
        {
            _cts.Cancel();
            _cts.Dispose();
            _cts = null;

            _reader?.Dispose();
            _reader = null;

            _writer?.Dispose();
            _writer = null;

            _tcpClient.Close();
            _tcpClient.Dispose();

            _messageSubject.OnCompleted();
            _messageSubject.Dispose();
        }
    }
}
