using NUnit.Framework;
using UniTwitchClient.EventSub;
using UniTwitchClient.EventSub.WebSocket;

namespace UniTwitchClient.Tests.EventSub
{
    public class ChannelFollowHandlerTest
    {
        [Test]
        public void HandleChannelFollowTest() 
        {
            var notification = new Notification()
            {
                UserId = "1234",
                UserLogin = "1234",
                UserName = "testUser",
            };

            ChannelFollow result = null;
            INotificationHandler handler = new ChannelFollowHandler(args => result = args);
            handler.HandleNotification(notification);
            
            Assert.IsNotNull(result);
            Assert.AreEqual("testUser", result.UserName);
            Assert.AreEqual("1234", result.UserId);
            Assert.AreEqual("1234", result.UserLogin);
        }
    }
}
