using NUnit.Framework;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UniTwitchClient.EventSub;
using UniTwitchClient.EventSub.Api;
using UniTwitchClient.EventSub.Api.Models;
using UniTwitchClient.EventSub.Api.Models.Raws;
using UnityEngine;

namespace UniTwitchClient.Tests.EventSub.Api
{
    public class ConvertEventSubSubscriptionDataTest
    {
        private readonly string data = "{\"total\":3,\"total_cost\":0,\"max_total_cost\":10,\"pagination\":{},\"data\":[{\"id\":\"4b073b17-a1c9-fedf-5023-63cbb308c9c5\",\"status\":\"enabled\",\"type\":\"channel.follow\",\"version\":\"1\",\"created_at\":\"2024-03-25T05:31:48.0319049Z\",\"cost\":0,\"condition\":{\"broadcaster_user_id\":\"1234\",\"moderator_user_id\":\"1234\"},\"transport\":{\"method\":\"websocket\",\"session_id\":\"db775189_a299dfaa\",\"connected_at\":\"2024-03-25T05:31:46.9629224Z\"}},{\"id\":\"85cad4fe-b154-32ed-d51c-90b7ff8810d1\",\"status\":\"enabled\",\"type\":\"channel.subscribe\",\"version\":\"1\",\"created_at\":\"2024-03-25T05:31:48.0432683Z\",\"cost\":0,\"condition\":{\"broadcaster_user_id\":\"1234\"},\"transport\":{\"method\":\"websocket\",\"session_id\":\"db775189_a299dfaa\",\"connected_at\":\"2024-03-25T05:31:46.9629224Z\"}},{\"id\":\"68bc74dd-507b-cfcb-0e0c-108a80dd42f8\",\"status\":\"enabled\",\"type\":\"channel.channel_points_custom_reward_redemption.add\",\"version\":\"1\",\"created_at\":\"2024-03-25T05:31:48.0460774Z\",\"cost\":0,\"condition\":{\"broadcaster_user_id\":\"1234\"},\"transport\":{\"method\":\"websocket\",\"session_id\":\"db775189_a299dfaa\",\"connected_at\":\"2024-03-25T05:31:46.9629224Z\"}}]}";

        [Test]
        public void ConvertEventSubSubscriptionDataRawTest() 
        {
            subscription_data subscription_data = JsonWrapper.ConvertFromJson<subscription_data>(data);

            Assert.IsNotNull(subscription_data);
            Assert.AreEqual(3, subscription_data.total);
            Assert.AreEqual("4b073b17-a1c9-fedf-5023-63cbb308c9c5", subscription_data.data[0].id);
            Assert.AreEqual("85cad4fe-b154-32ed-d51c-90b7ff8810d1", subscription_data.data[1].id);
            Assert.AreEqual("68bc74dd-507b-cfcb-0e0c-108a80dd42f8", subscription_data.data[2].id);

            Assert.AreEqual("1234", subscription_data.data[0].condition.broadcaster_user_id);
            Assert.AreEqual("1234", subscription_data.data[1].condition.broadcaster_user_id);
            Assert.AreEqual("1234", subscription_data.data[2].condition.broadcaster_user_id);

            Assert.AreEqual("db775189_a299dfaa", subscription_data.data[0].transport.session_id);
            Assert.AreEqual("db775189_a299dfaa", subscription_data.data[1].transport.session_id);
            Assert.AreEqual("db775189_a299dfaa", subscription_data.data[2].transport.session_id);
        }

        [Test]
        public void ConvertEventSubscriptionDataRawToModelTest() 
        {
            subscription_data subscription_data_raw = JsonWrapper.ConvertFromJson<subscription_data>(data);
            EventSubSubscriptionData eventSubSubscriptionData = subscription_data_raw.ConvertRawToModel();

            Assert.AreEqual("4b073b17-a1c9-fedf-5023-63cbb308c9c5", eventSubSubscriptionData.Subscriptions[0].Id);
            Assert.AreEqual("85cad4fe-b154-32ed-d51c-90b7ff8810d1", eventSubSubscriptionData.Subscriptions[1].Id);
            Assert.AreEqual("68bc74dd-507b-cfcb-0e0c-108a80dd42f8", eventSubSubscriptionData.Subscriptions[2].Id);
        }

        [Test]
        public void ConvertEventSubscriptionDataRawFromBlankDataToModelTest()
        {
            subscription_data subscription_data_raw = JsonWrapper.ConvertFromJson<subscription_data>("{}");
            EventSubSubscriptionData eventSubSubscriptionData = subscription_data_raw.ConvertRawToModel();

            Assert.IsNotNull(eventSubSubscriptionData);
        }

        [Test]
        public void ConvertEventSubSubscriptionTest() 
        {
            subscription_data subscription_data = JsonWrapper.ConvertFromJson<subscription_data>(data);
            subscription subscription_raw = subscription_data.data[0];
            var eventSubSubscription = subscription_raw.ConvertRawToModel();

            Assert.AreEqual("4b073b17-a1c9-fedf-5023-63cbb308c9c5", eventSubSubscription.Id);
            Assert.AreEqual("db775189_a299dfaa", eventSubSubscription.SessionId);
        }
    }
}