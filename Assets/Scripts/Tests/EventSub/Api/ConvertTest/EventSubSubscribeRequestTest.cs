using NUnit.Framework;
using UniTwitchClient.EventSub;
using UniTwitchClient.EventSub.Api.Models;
using UniTwitchClient.EventSub.Api.Models.Raws;

namespace UniTwitchClient.Tests.EventSub.Api
{
    public class EventSubSubscribeRequestTest
    {
        [Test]
        public void ConvertEventSubSubscribeRequestToJsonTest()
        {
            var session_id = "AQoQILE98gtqShGmLD7AM6yJThAB";
            var condition = new Condition()
            {
                BroadcasterUserId = "1234",
                ModeratorUserId = "2345",
                UserId = "123456",
                FromBroadcasterUserId = "01234",
                ToBroadcasterUserId = "98765",
            };

            var eventSubSubscribeRequest = new EventSubSubscribeRequest(SubscriptionType.ChannelSubscribe, condition);
            eventSubSubscribeRequest.AddSessionId(session_id);
            var json = eventSubSubscribeRequest.ToJson();
            var rawModel = JsonWrapper.ConvertFromJson<request_subscription_json>(json);

            Assert.AreEqual(SubscriptionType.ChannelSubscribe, SubscriptionTypeConverter.ToSubscriptionType(rawModel.type));
            Assert.AreEqual("1", rawModel.version);
            Assert.AreEqual(session_id, rawModel.transport.session_id);
            Assert.AreEqual(condition.BroadcasterUserId, rawModel.condition.broadcaster_user_id);
            Assert.AreEqual(condition.ModeratorUserId, rawModel.condition.moderator_user_id);
            Assert.AreEqual(condition.UserId, rawModel.condition.user_id);
            Assert.AreEqual(condition.FromBroadcasterUserId, rawModel.condition.from_broadcaster_user_id);
            Assert.AreEqual(condition.ToBroadcasterUserId, rawModel.condition.to_broadcaster_user_id);
        }

        [Test]
        public void ChannelFollowWillBeVersion2Test() 
        {
            var eventSubSubscribeRequest = new EventSubSubscribeRequest(SubscriptionType.ChannelFollow, new Condition());
            var json = eventSubSubscribeRequest.ToJson();
            var rawModel = JsonWrapper.ConvertFromJson<request_subscription_json>(json);
            Assert.AreEqual("2", rawModel.version);
        }

        [Test]
        public void BlankConditionShouldntBeIncludeParametersTest()
        {
            EventSubSubscribeRequest eventSubSubscribeRequest = new EventSubSubscribeRequest(SubscriptionType.None, new Condition());
            var json = eventSubSubscribeRequest.ToJson();
            Assert.IsTrue(json.Contains("\"condition\":{}"));
        }
    }
}