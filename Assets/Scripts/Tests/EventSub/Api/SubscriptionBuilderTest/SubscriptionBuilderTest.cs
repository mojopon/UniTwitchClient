using NUnit.Framework;
using UniTwitchClient.EventSub.Api;

namespace UniTwitchClient.Tests.EventSub.Api
{
    public class SubscriptionBuilderTest
    {
        private SubscriptionBuilder builder;
        private const string sessionId = "AQoQILE98gtqShGmLD7AM6yJThAB";

        [SetUp]
        public void SetUp()
        {
            builder = new SubscriptionBuilder();
        }

        [Test]
        public void SubscribeChannelFollowTest()
        {
            var broadCasterUserId = "12345";
            builder.SubscribeChannelFollow(broadCasterUserId);

            var subscriptions = builder.GetSubscriptionsWithSessionId(sessionId);

            Assert.AreEqual(SubscriptionType.ChannelFollow, subscriptions[0].SubscriptionType);
            Assert.AreEqual(sessionId, subscriptions[0].SessionId);
            Assert.AreEqual(broadCasterUserId, subscriptions[0].Condition.BroadcasterUserId);
        }

        [Test]
        public void SubscribeChannelSubscribeTest()
        {
            var broadCasterUserId = "12345";
            builder.SubscribeChannelSubscribe(broadCasterUserId);

            var subscriptions = builder.GetSubscriptionsWithSessionId(sessionId);

            Assert.AreEqual(SubscriptionType.ChannelSubscribe, subscriptions[0].SubscriptionType);
            Assert.AreEqual(sessionId, subscriptions[0].SessionId);
            Assert.AreEqual(broadCasterUserId, subscriptions[0].Condition.BroadcasterUserId);
        }

        [Test]
        public void SubscribeAllTest()
        {
            var broadCasterUserId = "12345";
            builder.SubscribeChannelSubscribe(broadCasterUserId);
            builder.SubscribeChannelFollow(broadCasterUserId);

            var subscriptions = builder.GetSubscriptionsWithSessionId(sessionId);

            Assert.AreEqual(SubscriptionType.ChannelSubscribe, subscriptions[0].SubscriptionType);
            Assert.AreEqual(sessionId, subscriptions[0].SessionId);
            Assert.AreEqual(broadCasterUserId, subscriptions[0].Condition.BroadcasterUserId);
            Assert.AreEqual(SubscriptionType.ChannelFollow, subscriptions[1].SubscriptionType);
            Assert.AreEqual(sessionId, subscriptions[1].SessionId);
            Assert.AreEqual(broadCasterUserId, subscriptions[1].Condition.BroadcasterUserId);
        }
    }
}
