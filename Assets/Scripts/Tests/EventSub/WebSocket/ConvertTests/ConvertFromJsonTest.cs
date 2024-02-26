using NUnit.Framework;
using UniTwitchClient.EventSub.WebSocket.Models.Raws;
using UniTwitchClient.EventSub;

namespace UniTwitchClient.Tests.EventSub.WebSocket
{
    public class ConvertFromJsonTest
    {
        [Test]
        public void ConvertWelcomeMessageTest()
        {
            string data = "{\"metadata\":{\"message_id\":\"cee70190-9024-df2f-3623-83f5ac0813aa\",\"message_type\":\"session_welcome\",\"message_timestamp\":\"2023-12-06T04:13:19.1999374Z\"},\"payload\":{\"session\":{\"id\":\"a07f635d_7b043750\",\"status\":\"connected\",\"keepalive_timeout_seconds\":10,\"reconnect_url\":null,\"connected_at\":\"2023-12-06T04:13:19.1989356Z\"}}}";
            var rawModel = JsonWrapper.ConvertFromJson<welcome_raw>(data);
            Assert.AreEqual("cee70190-9024-df2f-3623-83f5ac0813aa", rawModel.metadata.message_id);
            Assert.AreEqual("session_welcome", rawModel.metadata.message_type);
            Assert.AreEqual("a07f635d_7b043750", rawModel.payload.session.id);
            Assert.AreEqual("connected", rawModel.payload.session.status);
            Assert.AreEqual(10, rawModel.payload.session.keepalive_timeout_seconds);
            Assert.IsTrue(string.IsNullOrEmpty(rawModel.payload.session.reconnect_url));
        }

        [Test]
        public void ConvertKeepAliveMessageTest()
        {
            string data = "{\"metadata\":{\"message_id\":\"6fe2fbb5-ff4b-6498-c810-1fb7b4283775\",\"message_type\":\"session_keepalive\",\"message_timestamp\":\"2023-12-06T04:38:28.72119Z\"},\"payload\":{}}";
            var rawModel = JsonWrapper.ConvertFromJson<keepalive_raw>(data);
            Assert.AreEqual("6fe2fbb5-ff4b-6498-c810-1fb7b4283775", rawModel.metadata.message_id);
            Assert.AreEqual("session_keepalive", rawModel.metadata.message_type);
        }

        [Test]
        public void ConvertNotificationMessageTest()
        {
            string data = "{\"metadata\":{\"message_id\":\"a7a48955-763c-7c2e-3bef-f15be22adfae\",\"message_type\":\"notification\",\"message_timestamp\":\"2023-12-06T04:37:14.7861902Z\",\"subscription_type\":\"channel.follow\",\"subscription_version\":\"1\"},\"payload\":{\"subscription\":{\"id\":\"18e4300b-3c7c-de53-e539-e14dd27a3cc2\",\"status\":\"enabled\",\"type\":\"channel.follow\",\"version\":\"1\",\"condition\":{\"broadcaster_user_id\":\"59784628\"},\"transport\":{\"method\":\"websocket\",\"session_id\":\"a07f635d_f5563019\"},\"created_at\":\"2023-12-06T04:36:49.1212917Z\",\"cost\":0},\"event\":{\"broadcaster_user_id\":\"59784628\",\"broadcaster_user_login\":\"59784628\",\"broadcaster_user_name\":\"testBroadcaster\",\"followed_at\":\"2023-12-06T04:37:14.7766894Z\",\"user_id\":\"53514154\",\"user_login\":\"testFromUser\",\"user_name\":\"testFromUser\"}}}";
            var rawModel = JsonWrapper.ConvertFromJson<notification_raw>(data);
            Assert.AreEqual("a7a48955-763c-7c2e-3bef-f15be22adfae", rawModel.metadata.message_id);
            Assert.AreEqual("notification", rawModel.metadata.message_type);
            Assert.AreEqual("channel.follow", rawModel.metadata.subscription_type);
            Assert.AreEqual("1", rawModel.metadata.subscription_version);
            Assert.AreEqual("18e4300b-3c7c-de53-e539-e14dd27a3cc2", rawModel.payload.subscription.id);
            Assert.AreEqual("enabled", rawModel.payload.subscription.status);
            Assert.AreEqual("channel.follow", rawModel.payload.subscription.type);
            Assert.AreEqual("1", rawModel.payload.subscription.version);
            Assert.AreEqual("59784628", rawModel.payload.@event.broadcaster_user_id);
            Assert.AreEqual("59784628", rawModel.payload.@event.broadcaster_user_login);
            Assert.AreEqual("testBroadcaster", rawModel.payload.@event.broadcaster_user_name);
            Assert.AreEqual("53514154", rawModel.payload.@event.user_id);
            Assert.AreEqual("testFromUser", rawModel.payload.@event.user_login);
            Assert.AreEqual("testFromUser", rawModel.payload.@event.user_name);
        }
    }
}