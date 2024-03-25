using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UniTwitchClient.EventSub;
using UnityEngine;

namespace UniTwitchClient.Tests.EventSub.Api
{
    public class ConvertSubscriptionDataTest
    {
        [Test]
        public void ConvertSubscriptionDataRawTest() 
        {
            var data = "{\"total\":3,\"total_cost\":0,\"max_total_cost\":10,\"pagination\":{},\"data\":[{\"id\":\"4b073b17-a1c9-fedf-5023-63cbb308c9c5\",\"status\":\"enabled\",\"type\":\"channel.follow\",\"version\":\"1\",\"created_at\":\"2024-03-25T05:31:48.0319049Z\",\"cost\":0,\"condition\":{\"broadcaster_user_id\":\"1234\",\"moderator_user_id\":\"1234\"},\"transport\":{\"method\":\"websocket\",\"session_id\":\"db775189_a299dfaa\",\"connected_at\":\"2024-03-25T05:31:46.9629224Z\"}},{\"id\":\"85cad4fe-b154-32ed-d51c-90b7ff8810d1\",\"status\":\"enabled\",\"type\":\"channel.subscribe\",\"version\":\"1\",\"created_at\":\"2024-03-25T05:31:48.0432683Z\",\"cost\":0,\"condition\":{\"broadcaster_user_id\":\"1234\"},\"transport\":{\"method\":\"websocket\",\"session_id\":\"db775189_a299dfaa\",\"connected_at\":\"2024-03-25T05:31:46.9629224Z\"}},{\"id\":\"68bc74dd-507b-cfcb-0e0c-108a80dd42f8\",\"status\":\"enabled\",\"type\":\"channel.channel_points_custom_reward_redemption.add\",\"version\":\"1\",\"created_at\":\"2024-03-25T05:31:48.0460774Z\",\"cost\":0,\"condition\":{\"broadcaster_user_id\":\"1234\"},\"transport\":{\"method\":\"websocket\",\"session_id\":\"db775189_a299dfaa\",\"connected_at\":\"2024-03-25T05:31:46.9629224Z\"}}]}";

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
    }
}