using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UniTwitchClient.EventSub;
using UniTwitchClient.EventSub.Api;
using UniTwitchClient.EventSub.Api.Mocks;
using UniTwitchClient.EventSub.Mocks;
using UniTwitchClient.EventSub.WebSocket;
using Notification = UniTwitchClient.EventSub.WebSocket.Notification;
using UnityEngine;
using UniRx;
using System;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;

namespace UniTwitchClient.Tests.EventSub
{
    public class TwitchEventSubClientTest
    {
        private TwitchEventSubWebSocketClientMock _wsClient;
        private TwitchEventSubApiClientMock _apiClient;
        private TwitchEventSubClient _client;
        private CompositeDisposable _disposables;

        [SetUp]
        public void SetUp() 
        {
            _disposables = new CompositeDisposable();

            _wsClient = new TwitchEventSubWebSocketClientMock().AddTo(_disposables);
            _apiClient = new TwitchEventSubApiClientMock();
            _client = new TwitchEventSubClient(_wsClient, _apiClient).AddTo(_disposables);
        }

        [TearDown]
        public void TearDown() 
        {
            _disposables?.Dispose();
        }

        private readonly string _broadcasterUserId = "abcde";
        private readonly string _sessionId = "abcsession";
        [Test]
        public async Task ConnectTest() 
        {
            // UniTask.Yieldで1フレーム後にTwitchEventSubClientにWebSocketClient経由でセッション情報を通知
            UniTask.Create(async () =>
            {
                await UniTask.Yield();
                var welcomeMessage = new Welcome() { SessionId = _sessionId };
                _wsClient.ReceiveWelcomeMessage(welcomeMessage);
            }).Forget();

            await _client.ConnectChannelAsync(_broadcasterUserId);

            // ITwitchEventSubWebsocketClient.ConnectAsync()が呼ばれている事を確認
            Assert.IsTrue(_wsClient.IsConnected);

            // ITwitchEventSubApiClient.CreateEventSubSubscriptionsAsync(broadcasterUserId, sessionId)が正しく呼ばれていることを確認
            Assert.AreEqual(TwitchEventSubApiCalledMethodLog.MethodType.Create, _apiClient.CalledMethods[0].Type);
            Assert.AreEqual(_broadcasterUserId, _apiClient.CalledMethods[0].Parameters["broadcasterUserId"]);
            Assert.AreEqual(_sessionId, _apiClient.CalledMethods[0].Parameters["sessionId"]);
            Assert.AreEqual(_broadcasterUserId, _apiClient.CalledMethods[0].Parameters["moderatorUserId"]);
        }

        [Test]
        public async Task ConnectErrorTest() 
        {
            Exception error = null;
            var subscription = _client.OnErrorAsObservable.Subscribe(x => error = x);

            UniTask.Create(async () =>
            {
                await UniTask.Yield();
                var error = new Exception("an error has occured.");
                _wsClient.ReceiveError(error);
            }).Forget();

            try
            {
                await _client.ConnectChannelAsync(_broadcasterUserId);
            }
            catch(Exception ex)
            {

            }

            Assert.Pass();
            Assert.IsNotNull(error);
        }

        [Test]
        public async Task DisconnectTest() 
        {
            await ConnectTest();
            await _client.DisconnectChannelAsync();

            // ITwitchEventSubWebsocketClient.DisconnectAsync()が呼ばれている事を確認
            Assert.IsFalse(_wsClient.IsConnected);

            // ITwitchEventSubApiClient.DeleteEventSubSubscriptionsAsync(sessionId)が正しく呼ばれていることを確認
            Assert.AreEqual(TwitchEventSubApiCalledMethodLog.MethodType.Delete, _apiClient.CalledMethods[1].Type);
            Assert.AreEqual(_sessionId, _apiClient.CalledMethods[1].Parameters["sessionId"]);
        }

        [Test]
        public void ReceiveChannelFollowTest() 
        {
            ChannelFollow result = null;
            _client.OnChannelFollowAsObservable.Subscribe(x => 
            {
                result = x; 
            });

            var notification = new Notification()
            {
                SubscriptionType = SubscriptionType.ChannelFollow,
                UserId = "1234",
                UserLogin = "cool_user",
                UserName = "Cool_User",
                BroadCasterUserId = "1337",
                BroadCasterUserLogin = "cooler_user",
                BroadCasterUserName = "Cooler_User",
                FollowedAt = new DateTime(2023, 12, 21, 23, 54, 12),
            };
            _wsClient.ReceiveNotification(notification);

            Assert.IsNotNull(result);
            Assert.AreEqual("1234", result.UserId);
            Assert.AreEqual("cool_user", result.UserLogin);
            Assert.AreEqual("Cool_User", result.UserName);
            Assert.AreEqual("1337", result.BroadcasterUserId);
            Assert.AreEqual("cooler_user", result.BroadcasterUserLogin);
            Assert.AreEqual("Cooler_User", result.BroadcasterUserName);

            var targetTime = new DateTime(2023, 12, 21, 23, 54, 12);
            Assert.AreEqual(0, targetTime.CompareTo(result.FollowedAt));
        }

        [Test]
        public void ReceiveChannelSubscribeTest() 
        {
            ChannelSubscribe result = null;
            _client.OnChannelSubscribeAsObservable.Subscribe(x =>
            {
                result = x;
            });

            var notification = new Notification()
            {
                SubscriptionType = SubscriptionType.ChannelSubscribe,
                UserId = "1234",
                UserLogin = "cool_user",
                UserName = "Cool_User",
                BroadCasterUserId = "1337",
                BroadCasterUserLogin = "cooler_user",
                BroadCasterUserName = "Cooler_User",
                Tier = "1000",
                IsGift = true,
            };
            _wsClient.ReceiveNotification(notification);

            Assert.IsNotNull(result);
            Assert.AreEqual("1234", result.UserId);
            Assert.AreEqual("cool_user", result.UserLogin);
            Assert.AreEqual("Cool_User", result.UserName);
            Assert.AreEqual("1337", result.BroadcasterUserId);
            Assert.AreEqual("cooler_user", result.BroadcasterUserLogin);
            Assert.AreEqual("Cooler_User", result.BroadcasterUserName);
            Assert.AreEqual("1000", result.Tier);
            Assert.AreEqual(true, result.IsGift);
        }

        [Test]
        public void ReceiveChannelSubscriptionMessageTest() 
        {
            ChannelSubscriptionMessage result = null;
            _client.OnChannelSubscriptionMessageAsObservable.Subscribe(x =>
            {
                result = x;
            });

            var notification = new Notification()
            {
                SubscriptionType = SubscriptionType.ChannelSubscriptionMessage,
                UserId = "1234",
                UserLogin = "testUserLogin",
                UserName = "testUserName",
                BroadCasterUserId = "2345",
                BroadCasterUserLogin = "broadcasterUserLogin",
                BroadCasterUserName = "broadcasterUserName",
                Tier = "1000",
                Message = new Message()
                {
                    Text = "Love the stream! FevziGG",
                    Emotes = new List<Emote>() { new Emote() { Begin = 23, End = 30, Id = "302976485" } }
                },
                CumulativeMonths = 15,
                StreakMonths = 1,
                DurationMonths = 6,
            };
            _wsClient.ReceiveNotification(notification);

            Assert.IsNotNull(result);
            Assert.AreEqual("1234", result.UserId);
            Assert.AreEqual("testUserLogin", result.UserLogin);
            Assert.AreEqual("testUserName", result.UserName);

            Assert.AreEqual("2345", result.BroadcasterUserId);
            Assert.AreEqual("broadcasterUserLogin", result.BroadcasterUserLogin);
            Assert.AreEqual("broadcasterUserName", result.BroadcasterUserName);

            Assert.AreEqual("1000", result.Tier);

            Assert.AreEqual("Love the stream! FevziGG", result.Message.Text);
            Assert.AreEqual(1, result.Message.Emotes.Count);
            Assert.AreEqual(23, result.Message.Emotes[0].Begin);
            Assert.AreEqual(30, result.Message.Emotes[0].End);
            Assert.AreEqual("302976485", result.Message.Emotes[0].Id);

            Assert.AreEqual(15, result.CumulativeMonths);
            Assert.AreEqual(1, result.StreakMonths);
            Assert.AreEqual(6, result.DurationMonths);
        }

        [Test]
        public void ReceiveChannelCheerTest() 
        {
            ChannelCheer result = null;
            _client.OnChannelCheerAsObservable.Subscribe(x =>
            {
                result = x;
            });

            var notification = new Notification()
            {
                SubscriptionType = SubscriptionType.ChannelCheer,
                UserId = "1234",
                UserLogin = "testUserLogin",
                UserName = "testUserName",
                BroadCasterUserId = "2345",
                BroadCasterUserLogin = "broadcasterUserLogin",
                BroadCasterUserName = "broadcasterUserName",
                Message = new Message()
                {
                    Text = "This is a test event.",
                },
                Bits = 100,
            };
            _wsClient.ReceiveNotification(notification);

            Assert.IsNotNull(result);

            Assert.AreEqual("1234", result.UserId);
            Assert.AreEqual("testUserLogin", result.UserLogin);
            Assert.AreEqual("testUserName", result.UserName);

            Assert.AreEqual("2345", result.BroadcasterUserId);
            Assert.AreEqual("broadcasterUserLogin", result.BroadcasterUserLogin);
            Assert.AreEqual("broadcasterUserName", result.BroadcasterUserName);

            Assert.AreEqual(100, result.Bits);
            Assert.AreEqual("This is a test event.", result.Message);
        }

        [Test]
        public void ReceiveChannelPointsCustomRewardRedemptionAddTest() 
        {
            ChannelPointsCustomRewardRedemptionAdd result = null;
            _client.OnChannelPointsCustomRewardRedemptionAddAsObservable.Subscribe(x =>
            {
                result = x;
            });

            var notification = new Notification()
            {
                SubscriptionType = SubscriptionType.ChannelPointsCustomRewardRedemptionAdd,
                BroadCasterUserId = "86630555",
                BroadCasterUserName = "testBroadcaster",
                BroadCasterUserLogin = "testBroadcaster",
                UserId = "71310683",
                UserName = "testFromUser",
                UserLogin = "testFromUser",
                RewardCost = 150,
                RewardPrompt = "RedeemYourTestRewardfromCLI",
                RewardTitle = "TestRewardfromCLI",
                RedeemedAt = new System.DateTime(2024, 3, 8, 7, 58, 4),
            };
            _wsClient.ReceiveNotification(notification);

            Assert.IsNotNull(result);
            Assert.AreEqual("71310683", result.UserId);
            Assert.AreEqual("testFromUser", result.UserName);
            Assert.AreEqual("testFromUser", result.UserLogin);
            Assert.AreEqual("86630555", result.BroadcasterUserId);
            Assert.AreEqual("testBroadcaster", result.BroadcasterUserName);
            Assert.AreEqual("testBroadcaster", result.BroadcasterUserLogin);
            Assert.AreEqual(150, result.RewardCost);
            Assert.AreEqual("RedeemYourTestRewardfromCLI", result.RewardPrompt);
            Assert.AreEqual("TestRewardfromCLI", result.RewardTitle);

            Assert.AreEqual(2024, result.RedeemedAt.Year);
            Assert.AreEqual(3, result.RedeemedAt.Month);
            Assert.AreEqual(8, result.RedeemedAt.Day);
            Assert.AreEqual(7, result.RedeemedAt.Hour);
            Assert.AreEqual(58, result.RedeemedAt.Minute);
            Assert.AreEqual(4, result.RedeemedAt.Second);
        }

        [Test]
        public void ReceiveBlankDataTest()
        {
            ChannelFollow channelFollow = null;
            _client.OnChannelFollowAsObservable.Subscribe(x =>
            {
                channelFollow = x;
            });
            ChannelSubscribe channelSubscribe = null;
            _client.OnChannelSubscribeAsObservable.Subscribe(x =>
            {
                channelSubscribe = x;
            });
            ChannelPointsCustomRewardRedemptionAdd channelPointsCustomRewardRedemptionAdd = null;
            _client.OnChannelPointsCustomRewardRedemptionAddAsObservable.Subscribe(x =>
            {
                channelPointsCustomRewardRedemptionAdd = x;
            });

            var notification = new Notification();
            // エラーが出ない事を確認。
            _wsClient.ReceiveNotification(notification);

            // データが通知されてない事を確認
            Assert.IsNull(channelFollow);
            Assert.IsNull(channelSubscribe);
            Assert.IsNull(channelPointsCustomRewardRedemptionAdd);
        }

        [Test]
        public void DisposeTest() 
        {
            _client.Dispose();
            Assert.IsTrue(_wsClient.IsDisposed);
        }
    }
}