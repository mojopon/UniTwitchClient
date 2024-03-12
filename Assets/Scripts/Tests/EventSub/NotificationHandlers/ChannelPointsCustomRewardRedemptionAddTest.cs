using NUnit.Framework;
using UniTwitchClient.EventSub;
using UniTwitchClient.EventSub.WebSocket;

namespace UniTwitchClient.Tests.EventSub
{
    public class ChannelPointsCustomRewardRedemptionAddHandlerTest
    {
        [Test]
        public void HandleChannelPointsCustomRewardRedemptionAddTest() 
        {
            var notification = new Notification()
            {
                BroadCasterUserId = "86630555",
                BroadCasterUserName = "testBroadcaster",
                BroadCasterUserLogin = "testBroadcaster",
                UserId = "71310683",
                UserName = "testFromUser",
                UserLogin = "testFromUser",
                RewardCost = 150,
                RewardPrompt = "RedeemYourTestRewardfromCLI",
                RewardTitle = "TestRewardfromCLI",
                RedeemedAt = new System.DateTime(2024, 3, 8, 7, 58, 4),
            };

            ChannelPointsCustomRewardRedemptionAdd result = null;
            INotificationHandler handler = new ChannelPointsCustomRewardRedemptionAddHandler(args => result = args);
            handler.HandleNotification(notification);

            Assert.IsNotNull(result);
            Assert.AreEqual("71310683", result.UserId);
            Assert.AreEqual("testFromUser", result.UserName);
            Assert.AreEqual("testFromUser", result.UserLogin);
            Assert.AreEqual("86630555", result.BroadcasterUserId);
            Assert.AreEqual("testBroadcaster", result.BroadcasterUserName);
            Assert.AreEqual("testBroadcaster", result.BroadcasterUserLogin);
            Assert.AreEqual(150, result.RewardCost);
            Assert.AreEqual("RedeemYourTestRewardfromCLI", result.RewardPrompt);
            Assert.AreEqual("TestRewardfromCLI", result.RewardTitle);

            Assert.AreEqual(2024, result.RedeemedAt.Year);
            Assert.AreEqual(3, result.RedeemedAt.Month);
            Assert.AreEqual(8, result.RedeemedAt.Day);
            Assert.AreEqual(7, result.RedeemedAt.Hour);
            Assert.AreEqual(58, result.RedeemedAt.Minute);
            Assert.AreEqual(4, result.RedeemedAt.Second);
        }
    }
}
