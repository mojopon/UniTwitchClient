using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UniTwitchClient.EventSub;
using UniTwitchClient.EventSub.Converters;
using UniTwitchClient.EventSub.WebSocket;
using UnityEngine;

namespace UniTwitchClient.Tests.EventSub
{
    public class NotificationConverterTest
    {
        [Test]
        public void ConvertChannelFollowTest()
        {
            var dateTime = new DateTime(2000, 8, 1);
            var notification = new Notification()
            {
                UserId = "1234",
                UserLogin = "testUserLogin",
                UserName = "testUserName",
                BroadCasterUserId = "2345",
                BroadCasterUserLogin = "broadcasterUserLogin",
                BroadCasterUserName = "broadcasterUserName",
                FollowedAt = dateTime,
            };

            var converter = new ChannelFollowConverter();
            var result = converter.Convert(notification) as ChannelFollow;

            Assert.IsNotNull(result);

            Assert.AreEqual("1234", result.UserId);
            Assert.AreEqual("testUserLogin", result.UserLogin);
            Assert.AreEqual("testUserName", result.UserName);

            Assert.AreEqual("2345", result.BroadcasterUserId);
            Assert.AreEqual("broadcasterUserLogin", result.BroadcasterUserLogin);
            Assert.AreEqual("broadcasterUserName", result.BroadcasterUserName);

            Assert.AreEqual(2000, dateTime.Year);
            Assert.AreEqual(8, dateTime.Month);
            Assert.AreEqual(1, dateTime.Day);
        }

        [Test]
        public void ConvertChannelSubscribeTest()
        {
            var notification = new Notification()
            {
                UserId = "1234",
                UserLogin = "testUserLogin",
                UserName = "testUserName",
                BroadCasterUserId = "2345",
                BroadCasterUserLogin = "broadcasterUserLogin",
                BroadCasterUserName = "broadcasterUserName",
                Tier = "1000",
                IsGift = true,
            };

            var converter = new ChannelSubscribeConverter();
            var result = converter.Convert(notification) as ChannelSubscribe;

            Assert.IsNotNull(result);

            Assert.AreEqual("1234", result.UserId);
            Assert.AreEqual("testUserLogin", result.UserLogin);
            Assert.AreEqual("testUserName", result.UserName);

            Assert.AreEqual("2345", result.BroadcasterUserId);
            Assert.AreEqual("broadcasterUserLogin", result.BroadcasterUserLogin);
            Assert.AreEqual("broadcasterUserName", result.BroadcasterUserName);

            Assert.AreEqual("1000", result.Tier);
            Assert.IsTrue(result.IsGift);
        }

        [Test]
        public void ConvertChannelSubscriptionMessageTest() 
        {
            var notification = new Notification()
            {
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
                    Emotes = new List<Emote>() { new Emote() { Begin= 23, End = 30, Id = "302976485" } }
                },
                CumulativeMonths = 15,
                StreakMonths = 1,
                DurationMonths = 6,
            };

            var converter = new ChannelSubscriptionMessageConverter();
            var result = converter.Convert(notification) as ChannelSubscriptionMessage;

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
        public void ConvertChannelPointsCustomRewardRedemptionAddTest()
        {
            var dateTime = new DateTime(2000, 8, 1);
            var notification = new Notification()
            {
                UserId = "1234",
                UserLogin = "testUserLogin",
                UserName = "testUserName",
                BroadCasterUserId = "2345",
                BroadCasterUserLogin = "broadcasterUserLogin",
                BroadCasterUserName = "broadcasterUserName",
                Status = "fulfilled",
                RewardCost = 150,
                RewardPrompt = "RedeemYourTestRewardfromCLI",
                RewardTitle = "TestRewardfromCLI",
                RedeemedAt = dateTime,
            };

            var converter = new ChannelPointsCustomRewardRedemptionAddConverter();
            var result = converter.Convert(notification) as ChannelPointsCustomRewardRedemptionAdd;

            Assert.IsNotNull(result);

            Assert.AreEqual("1234", result.UserId);
            Assert.AreEqual("testUserLogin", result.UserLogin);
            Assert.AreEqual("testUserName", result.UserName);

            Assert.AreEqual("2345", result.BroadcasterUserId);
            Assert.AreEqual("broadcasterUserLogin", result.BroadcasterUserLogin);
            Assert.AreEqual("broadcasterUserName", result.BroadcasterUserName);

            Assert.AreEqual("fulfilled", result.Status);
            Assert.AreEqual(150, result.RewardCost);
            Assert.AreEqual("RedeemYourTestRewardfromCLI", result.RewardPrompt);
            Assert.AreEqual("TestRewardfromCLI", result.RewardTitle);

            Assert.AreEqual(2000, result.RedeemedAt.Year);
            Assert.AreEqual(8, result.RedeemedAt.Month);
            Assert.AreEqual(1, result.RedeemedAt.Day);
        }
    }
}