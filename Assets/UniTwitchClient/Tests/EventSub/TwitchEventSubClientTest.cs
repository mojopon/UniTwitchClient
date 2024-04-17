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
            ChannelFollow channelFollow = null;
            _client.OnChannelFollowAsObservable.Subscribe(x => 
            {
                channelFollow = x; 
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

            Assert.IsNotNull(channelFollow);
            Assert.AreEqual("1234", channelFollow.UserId);
            Assert.AreEqual("cool_user", channelFollow.UserLogin);
            Assert.AreEqual("Cool_User", channelFollow.UserName);
            Assert.AreEqual("1337", channelFollow.BroadcasterUserId);
            Assert.AreEqual("cooler_user", channelFollow.BroadcasterUserLogin);
            Assert.AreEqual("Cooler_User", channelFollow.BroadcasterUserName);

            var targetTime = new DateTime(2023, 12, 21, 23, 54, 12);
            Assert.AreEqual(0, targetTime.CompareTo(channelFollow.FollowedAt));
        }

        [Test]
        public void ReceiveChannelSubscribeTest() 
        {
            ChannelSubscribe channelSubscribe = null;
            _client.OnChannelSubscribeAsObservable.Subscribe(x =>
            {
                channelSubscribe = x;
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

            Assert.IsNotNull(channelSubscribe);
            Assert.AreEqual("1234", channelSubscribe.UserId);
            Assert.AreEqual("cool_user", channelSubscribe.UserLogin);
            Assert.AreEqual("Cool_User", channelSubscribe.UserName);
            Assert.AreEqual("1337", channelSubscribe.BroadcasterUserId);
            Assert.AreEqual("cooler_user", channelSubscribe.BroadcasterUserLogin);
            Assert.AreEqual("Cooler_User", channelSubscribe.BroadcasterUserName);
        }

        [Test]
        public void ReceiveChannelSubscriptionMessageTest() 
        {

        }

        [Test]
        public void ReceiveChannelPointsCustomRewardRedemptionAddTest() 
        {
            ChannelPointsCustomRewardRedemptionAdd channelPointsCustomRewardRedemptionAdd = null;
            _client.OnChannelPointsCustomRewardRedemptionAddAsObservable.Subscribe(x =>
            {
                channelPointsCustomRewardRedemptionAdd = x;
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

            Assert.IsNotNull(channelPointsCustomRewardRedemptionAdd);
            Assert.AreEqual("71310683", channelPointsCustomRewardRedemptionAdd.UserId);
            Assert.AreEqual("testFromUser", channelPointsCustomRewardRedemptionAdd.UserName);
            Assert.AreEqual("testFromUser", channelPointsCustomRewardRedemptionAdd.UserLogin);
            Assert.AreEqual("86630555", channelPointsCustomRewardRedemptionAdd.BroadcasterUserId);
            Assert.AreEqual("testBroadcaster", channelPointsCustomRewardRedemptionAdd.BroadcasterUserName);
            Assert.AreEqual("testBroadcaster", channelPointsCustomRewardRedemptionAdd.BroadcasterUserLogin);
            Assert.AreEqual(150, channelPointsCustomRewardRedemptionAdd.RewardCost);
            Assert.AreEqual("RedeemYourTestRewardfromCLI", channelPointsCustomRewardRedemptionAdd.RewardPrompt);
            Assert.AreEqual("TestRewardfromCLI", channelPointsCustomRewardRedemptionAdd.RewardTitle);

            Assert.AreEqual(2024, channelPointsCustomRewardRedemptionAdd.RedeemedAt.Year);
            Assert.AreEqual(3, channelPointsCustomRewardRedemptionAdd.RedeemedAt.Month);
            Assert.AreEqual(8, channelPointsCustomRewardRedemptionAdd.RedeemedAt.Day);
            Assert.AreEqual(7, channelPointsCustomRewardRedemptionAdd.RedeemedAt.Hour);
            Assert.AreEqual(58, channelPointsCustomRewardRedemptionAdd.RedeemedAt.Minute);
            Assert.AreEqual(4, channelPointsCustomRewardRedemptionAdd.RedeemedAt.Second);
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