using NUnit.Framework;
using UniTwitchClient.EventSub.Api;

namespace UniTwitchClient.Tests.EventSub.Api
{
    public class EventSubSubscribeRequestBuilderTest
    {
        private EventSubSubscribeRequestBuilder builder;
        private const string sessionId = "AQoQILE98gtqShGmLD7AM6yJThAB";

        [SetUp]
        public void SetUp()
        {
            builder = new EventSubSubscribeRequestBuilder();
        }

        [Test]
        public void SubscribeChannelFollowTest()
        {
            var broadCasterUserId = "12345";
            builder.CreateSubscribeChannelFollowRequest(broadCasterUserId);

            var requests = builder.GetEventSubSubscribeRequestsWithSessionId(sessionId);

            Assert.AreEqual(SubscriptionType.ChannelFollow, requests[0].SubscriptionType);
            Assert.AreEqual(sessionId, requests[0].SessionId);
            Assert.AreEqual(broadCasterUserId, requests[0].Condition.BroadcasterUserId);
        }

        [Test]
        public void SubscribeChannelSubscribeTest()
        {
            var broadCasterUserId = "12345";
            builder.CreateSubscribeChannelSubscribeRequest(broadCasterUserId);

            var requests = builder.GetEventSubSubscribeRequestsWithSessionId(sessionId);

            Assert.AreEqual(SubscriptionType.ChannelSubscribe, requests[0].SubscriptionType);
            Assert.AreEqual(sessionId, requests[0].SessionId);
            Assert.AreEqual(broadCasterUserId, requests[0].Condition.BroadcasterUserId);
        }

        [Test]
        public void SubscribeChannelSubscriptionMessageTest() 
        {
            var broadCasterUserId = "12345";
            builder.CreateSubscribeChannelSubscriptionMessage(broadCasterUserId);

            var requests = builder.GetEventSubSubscribeRequestsWithSessionId(sessionId);

            Assert.AreEqual(SubscriptionType.ChannelSubscriptionMessage, requests[0].SubscriptionType);
            Assert.AreEqual(sessionId, requests[0].SessionId);
            Assert.AreEqual(broadCasterUserId, requests[0].Condition.BroadcasterUserId);
        }

        [Test]
        public void SubscribeAllTest()
        {
            var broadCasterUserId = "12345";
            var moderatorUserId = "23456";
            builder.CreateAllSubscriptionRequests(broadCasterUserId, moderatorUserId);
            var requests = builder.GetEventSubSubscribeRequestsWithSessionId(sessionId);

            Assert.AreEqual(4, requests.Length);
        }
    }
}
