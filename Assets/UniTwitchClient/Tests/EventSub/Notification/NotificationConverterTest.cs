using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UniTwitchClient.EventSub;
using UniTwitchClient.EventSub.WebSocket;
using UnityEngine;

namespace UniTwitchClient.Tests.EventSub
{
    public class NotificationConverterTest
    {
        [Test]
        public void ConvertChannelFollowTest() 
        {
            var dateTime = new DateTime(2000,8,1);
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
    }
}