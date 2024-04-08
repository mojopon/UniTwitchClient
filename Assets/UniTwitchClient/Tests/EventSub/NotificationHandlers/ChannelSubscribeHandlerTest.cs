using NUnit.Framework;
using UniTwitchClient.EventSub;
using UniTwitchClient.EventSub.WebSocket;

namespace UniTwitchClient.Tests.EventSub
{
    public class ChannelSubscribeHandlerTest
    {
        [Test]
        public void HandleChannelSubscribeTest()
        {
            var notification = new Notification()
            {
                UserId = "1234",
                UserLogin = "1234",
                UserName = "testUser",
                BroadCasterUserId = "1337",
                BroadCasterUserLogin = "cooler_user",
                BroadCasterUserName = "Cooler_User",
                Tier = "1000",
                IsGift = true,
            };

            ChannelSubscribe result = null;
            INotificationHandler handler = new ChannelSubscribeHandler(args => result = args);
            handler.HandleNotification(notification);

            Assert.IsNotNull(result);
            Assert.AreEqual("testUser", result.UserName);
            Assert.AreEqual("1234", result.UserId);
            Assert.AreEqual("1234", result.UserLogin);
            Assert.AreEqual("1337", result.BroadcasterUserId);
            Assert.AreEqual("cooler_user", result.BroadcasterUserLogin);
            Assert.AreEqual("Cooler_User", result.BroadcasterUserName);
            Assert.AreEqual("1000", result.Tier);
            Assert.AreEqual(true, result.IsGift);
        }
    }
}
