using NUnit.Framework;
using UniTwitchClient.EventSub;
using UniTwitchClient.EventSub.WebSocket;
using UniTwitchClient.EventSub.WebSocket.Models.Raws;

namespace UniTwitchClient.Tests.EventSub.WebSocket
{
    public class ConvertNotificationTest
    {
        [Test]
        public void ConvertNotificationFromBlankDataTest() 
        {
            string data = "{}";
            var rawModel = JsonWrapper.ConvertFromJson<notification_raw>(data);
            var model = rawModel.ConvertRawToModel();

            Assert.IsNotNull(model);
        }

        [Test]
        public void ConvertChannelFollowMessageToNotificationTest()
        {
            string data = "{\"metadata\":{\"message_id\":\"a7a48955-763c-7c2e-3bef-f15be22adfae\",\"message_type\":\"notification\",\"message_timestamp\":\"2023-12-06T04:37:14.7861902Z\",\"subscription_type\":\"channel.follow\",\"subscription_version\":\"1\"},\"payload\":{\"subscription\":{\"id\":\"18e4300b-3c7c-de53-e539-e14dd27a3cc2\",\"status\":\"enabled\",\"type\":\"channel.follow\",\"version\":\"1\",\"condition\":{\"broadcaster_user_id\":\"59784628\"},\"transport\":{\"method\":\"websocket\",\"session_id\":\"a07f635d_f5563019\"},\"created_at\":\"2023-12-06T04:36:49.1212917Z\",\"cost\":0},\"event\":{\"broadcaster_user_id\":\"59784628\",\"broadcaster_user_login\":\"59784628\",\"broadcaster_user_name\":\"testBroadcaster\",\"followed_at\":\"2023-12-06T04:37:14.7766894Z\",\"user_id\":\"53514154\",\"user_login\":\"testFromUser\",\"user_name\":\"testFromUser\"}}}";
            var rawModel = JsonWrapper.ConvertFromJson<notification_raw>(data);
            var model = rawModel.ConvertRawToModel();

            Assert.AreEqual("notification", model.MessageType);
            Assert.AreEqual("channel.follow", model.SubscriptionType.ToName());
            Assert.AreEqual("53514154", model.UserId);
            Assert.AreEqual("testFromUser", model.UserLogin);
            Assert.AreEqual("testFromUser", model.UserName);
            Assert.AreEqual("59784628", model.BroadCasterUserId);
            Assert.AreEqual("59784628", model.BroadCasterUserLogin);
            Assert.AreEqual("testBroadcaster", model.BroadCasterUserName);

            Assert.AreEqual(2023, model.MessageTimeStamp.Year);
            Assert.AreEqual(12, model.MessageTimeStamp.Month);
            Assert.AreEqual(6, model.MessageTimeStamp.Day);
            Assert.AreEqual(4, model.MessageTimeStamp.Hour);
            Assert.AreEqual(37, model.MessageTimeStamp.Minute);
            Assert.AreEqual(14, model.MessageTimeStamp.Second);

            Assert.AreEqual(2023, model.FollowedAt.Year);
            Assert.AreEqual(12, model.FollowedAt.Month);
            Assert.AreEqual(6, model.FollowedAt.Day);
            Assert.AreEqual(4, model.FollowedAt.Hour);
            Assert.AreEqual(37, model.FollowedAt.Minute);
            Assert.AreEqual(14, model.FollowedAt.Second);
        }

        [Test]
        public void ConvertChannelSubscribeMessageToNotificationTest() 
        {
            string data = "{\"metadata\":{\"message_id\":\"1e6411cf-f155-8a8a-d33f-fc8c2d27ae6d\",\"message_type\":\"notification\",\"message_timestamp\":\"2023-12-21T05:47:25.3020511Z\",\"subscription_type\":\"channel.subscribe\",\"subscription_version\":\"1\"},\"payload\":{\"subscription\":{\"id\":\"9a9b429e-4b6b-2664-9800-97803ab5effd\",\"status\":\"enabled\",\"type\":\"channel.subscribe\",\"version\":\"1\",\"condition\":{\"broadcaster_user_id\":\"1292476\"},\"transport\":{\"method\":\"websocket\",\"session_id\":\"d1eff6a2_1d67efcf\"},\"created_at\":\"2023-12-21T05:45:26.9711192Z\",\"cost\":0},\"event\":{\"broadcaster_user_id\":\"1292476\",\"broadcaster_user_login\":\"testBroadcaster\",\"broadcaster_user_name\":\"testBroadcaster\",\"is_gift\":true,\"tier\":\"1000\",\"user_id\":\"7816949\",\"user_login\":\"testFromUser\",\"user_name\":\"testFromUser\"}}}";
            var rawModel = JsonWrapper.ConvertFromJson<notification_raw>(data);
            var model = rawModel.ConvertRawToModel();

            Assert.AreEqual("notification", model.MessageType);
            Assert.AreEqual("channel.subscribe", model.SubscriptionType.ToName());
            Assert.AreEqual("7816949", model.UserId);
            Assert.AreEqual("testFromUser", model.UserLogin);
            Assert.AreEqual("testFromUser", model.UserName);
            Assert.AreEqual("1292476", model.BroadCasterUserId);
            Assert.AreEqual("testBroadcaster", model.BroadCasterUserLogin);
            Assert.AreEqual("testBroadcaster", model.BroadCasterUserName);
            Assert.AreEqual(true, model.IsGift);
            Assert.AreEqual("1000", model.Tier);

            Assert.AreEqual(2023, model.MessageTimeStamp.Year);
            Assert.AreEqual(12, model.MessageTimeStamp.Month);
            Assert.AreEqual(21, model.MessageTimeStamp.Day);
            Assert.AreEqual(5, model.MessageTimeStamp.Hour);
            Assert.AreEqual(47, model.MessageTimeStamp.Minute);
            Assert.AreEqual(25, model.MessageTimeStamp.Second);
        }
    }
}