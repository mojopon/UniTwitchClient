using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UniTwitchClient.Chat;
using UniTwitchClient.Chat.Mocks;
using UniTwitchClient.Chat.Models;
using UniRx;
using UnityEngine;

namespace UniTwitchClient.Tests.Chat
{
    public class TwitchChatClientTest
    {
        private readonly string CHANNEL_NAME = "testchannel";
        private readonly string TWITCH_BOT_NAME = "testuser";

        private readonly string message = "@badges=staff/1,broadcaster/1,turbo/1;color=#FF0000;display-name=PetsgomOO;emote-only=1;emotes=33:0-7;flags=0-7:A.6/P.6,25-36:A.1/I.2;id=c285c9ed-8b1b-4702-ae1c-c64d76cc74ef;mod=0;room-id=81046256;subscriber=0;turbo=0;tmi-sent-ts=1550868292494;user-id=81046256;user-type=staff :petsgomoo!petsgomoo@petsgomoo.tmi.twitch.tv PRIVMSG #petsgomoo :DansGame";

        private readonly string userAccessToken = "fisjiinntup756xcn7garu6drakenr";
        private readonly string userName = "testuser";
        private readonly string clientId = "bmny4dt43ufheb95zc3w82b557eana";

        private CompositeDisposable _compositeDisposable;
        private TwitchIrcCredentials _credentials;
        private TwitchIrcClientMock _mock;
        private TwitchChatClient _client;

        [SetUp]
        public void SetUp() 
        {
            _compositeDisposable = new CompositeDisposable();
            _credentials = new TwitchIrcCredentials(userAccessToken, userName);
            _mock = new TwitchIrcClientMock();
            _client = new TwitchChatClient(_credentials, _mock);
        }

        [TearDown]
        public void TearDown() 
        {
            _compositeDisposable.Dispose();
        }

        [Test]
        public void OnLoginAndCloseTest() 
        {
            Assert.AreEqual(ConnectionState.Idle, _client.State);

            _client.Connect(CHANNEL_NAME);

            Assert.AreEqual(ConnectionState.Connecting, _client.State);

            _mock.ReceiveMessage($":tmi.twitch.tv 001 {TWITCH_BOT_NAME} :Welcome, GLHF!");
            _mock.ReceiveMessage($":tmi.twitch.tv 002 {TWITCH_BOT_NAME} :Your host is tmi.twitch.tv");
            _mock.ReceiveMessage($":tmi.twitch.tv 003 {TWITCH_BOT_NAME} :This server is rather new");
            _mock.ReceiveMessage($":tmi.twitch.tv 004 {TWITCH_BOT_NAME} :-");
            _mock.ReceiveMessage($":tmi.twitch.tv 375 {TWITCH_BOT_NAME} :-");
            _mock.ReceiveMessage($":tmi.twitch.tv 372 {TWITCH_BOT_NAME} :You are in a maze of twisty passages, all alike.");
            _mock.ReceiveMessage($":tmi.twitch.tv 376 {TWITCH_BOT_NAME} :>");
            _mock.ReceiveMessage(":tmi.twitch.tv CAP * ACK :twitch.tv/tags");
            _mock.ReceiveMessage(":tmi.twitch.tv CAP * ACK :twitch.tv/commands");
            _mock.ReceiveMessage(":tmi.twitch.tv CAP * ACK :twitch.tv/membership");

            Assert.AreEqual(ConnectionState.Connected, _client.State);

            _client.Close();
            Assert.AreEqual(ConnectionState.Disconnected, _client.State);
        }

        [Test]
        public void ReceiveMessageTest() 
        {
            TwitchChatMessage result = null;
            string resultRaw = null;
            _client.TwitchChatMessageAsObservable.Subscribe(x => result = x).AddTo(_compositeDisposable);
            _client.MessageRawAsObservable.Subscribe(x => resultRaw = x).AddTo(_compositeDisposable);

            _mock.ReceiveMessage(message);

            Assert.AreEqual(message, resultRaw);
            Assert.IsNotNull(result);

            Assert.AreEqual("1", result.Badge.Staff);
            Assert.AreEqual("1", result.Badge.Broadcaster);
            Assert.AreEqual("1", result.Badge.Turbo);

            Assert.AreEqual("PetsgomOO", result.DisplayName);
            Assert.AreEqual("petsgomoo", result.UserNickname);
            Assert.AreEqual("#FF0000", result.Color);
            Assert.AreEqual("81046256", result.RoomId);
            Assert.AreEqual("81046256", result.UserId);

            Assert.AreEqual("33", result.Emotes[0].Id);
            Assert.AreEqual(0, result.Emotes[0].StartIndex);
            Assert.AreEqual(7, result.Emotes[0].EndIndex);

            Assert.AreEqual(TwitchIrcCommand.PrivMsg, result.Command);
            Assert.AreEqual("DansGame", result.Message);
        }
    }
}
