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
    }
}