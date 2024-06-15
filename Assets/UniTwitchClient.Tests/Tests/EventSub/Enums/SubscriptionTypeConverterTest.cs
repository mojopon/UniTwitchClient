using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniTwitchClient.Tests.EventSub
{
    public class SubscriptionTypeConverterTest
    {
        [Test]
        public void ConvertNameToTypeTest()
        {
            SubscriptionType type = SubscriptionType.None;

            type = SubscriptionTypeConverter.ToSubscriptionType("channel.update");
            Assert.AreEqual(SubscriptionType.ChannelUpdate, type);
            type = SubscriptionTypeConverter.ToSubscriptionType("channel.follow");
            Assert.AreEqual(SubscriptionType.ChannelFollow, type);
            type = SubscriptionTypeConverter.ToSubscriptionType("channel.cheer");
            Assert.AreEqual(SubscriptionType.ChannelCheer, type);
            type = SubscriptionTypeConverter.ToSubscriptionType("channel.subscribe");
            Assert.AreEqual(SubscriptionType.ChannelSubscribe, type);
            type = SubscriptionTypeConverter.ToSubscriptionType("channel.subscription.gift");
            Assert.AreEqual(SubscriptionType.ChannelSubscriptionGift, type);
            type = SubscriptionTypeConverter.ToSubscriptionType("channel.subscription.message");
            Assert.AreEqual(SubscriptionType.ChannelSubscriptionMessage, type);
            type = SubscriptionTypeConverter.ToSubscriptionType("channel.channel_points_custom_reward_redemption.add");
            Assert.AreEqual(SubscriptionType.ChannelPointsCustomRewardRedemptionAdd, type);
        }

        [Test]
        public void ConvertTypeToNameTest()
        {
            string name = "";

            name = SubscriptionTypeConverter.ToName(SubscriptionType.ChannelUpdate);
            Assert.AreEqual("channel.update", name);
            name = SubscriptionTypeConverter.ToName(SubscriptionType.ChannelFollow);
            Assert.AreEqual("channel.follow", name);
            name = SubscriptionTypeConverter.ToName(SubscriptionType.ChannelCheer);
            Assert.AreEqual("channel.cheer", name);
            name = SubscriptionTypeConverter.ToName(SubscriptionType.ChannelSubscribe);
            Assert.AreEqual("channel.subscribe", name);
            name = SubscriptionTypeConverter.ToName(SubscriptionType.ChannelSubscriptionGift);
            Assert.AreEqual("channel.subscription.gift", name);
            name = SubscriptionTypeConverter.ToName(SubscriptionType.ChannelSubscriptionMessage);
            Assert.AreEqual("channel.subscription.message", name);
            name = SubscriptionTypeConverter.ToName(SubscriptionType.ChannelPointsCustomRewardRedemptionAdd);
            Assert.AreEqual("channel.channel_points_custom_reward_redemption.add", name);
        }
    }
}