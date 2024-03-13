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