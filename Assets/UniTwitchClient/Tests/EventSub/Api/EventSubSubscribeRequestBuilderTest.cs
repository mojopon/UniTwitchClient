using NUnit.Framework;
using System.Linq;
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
        public void CreateChannelFollowTest()
        {
            var broadCasterUserId = "12345";
            builder.CreateChannelFollowRequest(broadCasterUserId);

            var requests = builder.BuildRequestsWithSessionId(sessionId);

            Assert.AreEqual(SubscriptionType.ChannelFollow, requests[0].SubscriptionType);
            Assert.AreEqual(sessionId, requests[0].SessionId);
            Assert.AreEqual(broadCasterUserId, requests[0].Condition.BroadcasterUserId);
        }

        [Test]
        public void CreateChannelSubscribeTest()
        {
            var broadCasterUserId = "12345";
            builder.CreateChannelSubscribeRequest(broadCasterUserId);

            var requests = builder.BuildRequestsWithSessionId(sessionId);

            Assert.AreEqual(SubscriptionType.ChannelSubscribe, requests[0].SubscriptionType);
            Assert.AreEqual(sessionId, requests[0].SessionId);
            Assert.AreEqual(broadCasterUserId, requests[0].Condition.BroadcasterUserId);
        }

        [Test]
        public void CreateChannelSubscriptionMessageTest() 
        {
            var broadCasterUserId = "12345";
            builder.CreateChannelSubscriptionMessage(broadCasterUserId);

            var requests = builder.BuildRequestsWithSessionId(sessionId);

            Assert.AreEqual(SubscriptionType.ChannelSubscriptionMessage, requests[0].SubscriptionType);
            Assert.AreEqual(sessionId, requests[0].SessionId);
            Assert.AreEqual(broadCasterUserId, requests[0].Condition.BroadcasterUserId);
        }

        [Test]
        public void CreateChannelPointsCustomRewardRedemptionAddRequest()
        {
            var broadCasterUserId = "12345";
            builder.CreateChannelPointsCustomRewardRedemptionAddRequest(broadCasterUserId);

            var requests = builder.BuildRequestsWithSessionId(sessionId);

            Assert.AreEqual(SubscriptionType.ChannelPointsCustomRewardRedemptionAdd, requests[0].SubscriptionType);
            Assert.AreEqual(sessionId, requests[0].SessionId);
            Assert.AreEqual(broadCasterUserId, requests[0].Condition.BroadcasterUserId);
        }

        [Test]
        public void CreateAllTest()
        {
            var broadCasterUserId = "12345";
            var moderatorUserId = "23456";
            builder.CreateAllRequests(broadCasterUserId, moderatorUserId);
            var requests = builder.BuildRequestsWithSessionId(sessionId);

            Assert.AreEqual(4, requests.Length);
            Assert.IsTrue(requests.Select(x => x.SubscriptionType).Contains(SubscriptionType.ChannelFollow));
            Assert.IsTrue(requests.Select(x => x.SubscriptionType).Contains(SubscriptionType.ChannelSubscribe));
            Assert.IsTrue(requests.Select(x => x.SubscriptionType).Contains(SubscriptionType.ChannelSubscriptionMessage));
            Assert.IsTrue(requests.Select(x => x.SubscriptionType).Contains(SubscriptionType.ChannelPointsCustomRewardRedemptionAdd));
        }
    }
}
