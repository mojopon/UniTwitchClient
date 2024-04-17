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
        public void ConvertChannelFollowToNotificationTest()
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
        public void ConvertChannelSubscribeToNotificationTest() 
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

        [Test]
        public void ConvertChannelSubscriptionMessageToNotificationTest() 
        {
            string data = "{\"metadata\":{\"message_id\":\"da8a86e6-d75c-4ac4-7150-4dd09588a92f\",\"message_type\":\"notification\",\"message_timestamp\":\"2024-04-16T05:47:31.4499994Z\",\"subscription_type\":\"channel.subscription.message\",\"subscription_version\":\"1\"},\"payload\":{\"subscription\":{\"id\":\"44cfc4e8-2dc5-9d66-a39e-bca631cf8ee5\",\"status\":\"enabled\",\"type\":\"channel.subscription.message\",\"version\":\"1\",\"condition\":{\"broadcaster_user_id\":\"31200102\"},\"transport\":{\"method\":\"websocket\",\"session_id\":\"4f376e8e_f85c8dd2\"},\"created_at\":\"2024-04-16T05:47:17.2429992Z\",\"cost\":0},\"event\":{\"broadcaster_user_id\":\"31200102\",\"broadcaster_user_login\":\"testBroadcaster\",\"broadcaster_user_name\":\"testBroadcaster\",\"cumulative_months\":79,\"duration_months\":1,\"message\":{\"emotes\":[{\"begin\":26,\"end\":39,\"id\":\"304456816\"}],\"text\":\"Hello from the Twitch CLI! twitchdevLeek\"},\"streak_months\":79,\"tier\":\"1000\",\"user_id\":\"93642253\",\"user_login\":\"testFromUser\",\"user_name\":\"testFromUser\"}}}";
            var rawModel = JsonWrapper.ConvertFromJson<notification_raw>(data);
            var model = rawModel.ConvertRawToModel();

            Assert.AreEqual("notification", model.MessageType);
            Assert.AreEqual("channel.subscription.message", model.SubscriptionType.ToName());
            Assert.AreEqual("93642253", model.UserId);
            Assert.AreEqual("testFromUser", model.UserLogin);
            Assert.AreEqual("testFromUser", model.UserName);
            Assert.AreEqual("31200102", model.BroadCasterUserId);
            Assert.AreEqual("testBroadcaster", model.BroadCasterUserLogin);
            Assert.AreEqual("testBroadcaster", model.BroadCasterUserName);

            Assert.AreEqual("Hello from the Twitch CLI! twitchdevLeek", model.Message.Text);

            Assert.AreEqual(1, model.Message.Emotes.Count);
            Assert.AreEqual(26, model.Message.Emotes[0].Begin);
            Assert.AreEqual(39, model.Message.Emotes[0].End);
            Assert.AreEqual("304456816", model.Message.Emotes[0].Id);

            Assert.AreEqual(79, model.CumulativeMonths);
            Assert.AreEqual(1, model.DurationMonths);
            Assert.AreEqual(79, model.StreakMonths);
        }

        [Test]
        public void ConvertChannelPointsCustomRewardRedemptionAddToNotificationTest() 
        {
            string data = "{\"metadata\":{\"message_id\":\"247cedf0-ad59-0399-990c-affd423a2acd\",\"message_type\":\"notification\",\"message_timestamp\":\"2024-03-08T07:58:04.2650485Z\",\"subscription_type\":\"channel.channel_points_custom_reward_redemption.add\",\"subscription_version\":\"1\"},\"payload\":{\"subscription\":{\"id\":\"860a5ef8-8288-2d61-0e72-a73258dc8fc8\",\"status\":\"enabled\",\"type\":\"channel.channel_points_custom_reward_redemption.add\",\"version\":\"1\",\"condition\":{\"broadcaster_user_id\":\"86630555\"},\"transport\":{\"method\":\"websocket\",\"session_id\":\"e3ffe25e_dfd480b9\"},\"created_at\":\"2024-03-08T07:57:55.66569Z\",\"cost\":0},\"event\":{\"broadcaster_user_id\":\"86630555\",\"broadcaster_user_login\":\"testBroadcaster\",\"broadcaster_user_name\":\"testBroadcaster\",\"id\":\"860a5ef8-8288-2d61-0e72-a73258dc8fc8\",\"redeemed_at\":\"2024-03-08T07:58:04.2444073Z\",\"reward\":{\"cost\":150,\"id\":\"d53ad741-f148-4a29-2e7f-355fffe73563\",\"prompt\":\"RedeemYourTestRewardfromCLI\",\"title\":\"TestRewardfromCLI\"},\"status\":\"unfulfilled\",\"user_id\":\"71310683\",\"user_input\":\"TestInputFromCLI\",\"user_login\":\"testFromUser\",\"user_name\":\"testFromUser\"}}}";
            var rawModel = JsonWrapper.ConvertFromJson<notification_raw>(data);
            var model = rawModel.ConvertRawToModel();

            Assert.AreEqual("notification", model.MessageType);
            Assert.AreEqual("channel.channel_points_custom_reward_redemption.add", model.SubscriptionType.ToName());
            Assert.AreEqual("71310683", model.UserId);
            Assert.AreEqual("testFromUser", model.UserLogin);
            Assert.AreEqual("testFromUser", model.UserName);
            Assert.AreEqual("86630555", model.BroadCasterUserId);
            Assert.AreEqual("testBroadcaster", model.BroadCasterUserLogin);
            Assert.AreEqual("testBroadcaster", model.BroadCasterUserName);

            Assert.AreEqual(150, model.RewardCost);
            Assert.AreEqual("RedeemYourTestRewardfromCLI", model.RewardPrompt);
            Assert.AreEqual("TestRewardfromCLI", model.RewardTitle);

            Assert.AreEqual(2024, model.RedeemedAt.Year);
            Assert.AreEqual(3, model.RedeemedAt.Month);
            Assert.AreEqual(8, model.RedeemedAt.Day);
            Assert.AreEqual(7, model.RedeemedAt.Hour);
            Assert.AreEqual(58, model.RedeemedAt.Minute);
            Assert.AreEqual(4, model.RedeemedAt.Second);
        }
    }
}