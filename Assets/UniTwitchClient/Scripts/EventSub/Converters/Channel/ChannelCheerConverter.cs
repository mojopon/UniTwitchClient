using UniTwitchClient.EventSub.WebSocket;

namespace UniTwitchClient.EventSub.Converters
{
    public class ChannelCheerConverter : INotificationConverter
    {
        public object Convert(Notification notification)
        {
            return new ChannelCheer(notification.IsAnonymous,
                                    notification.UserId,
                                    notification.UserName,
                                    notification.UserLogin,
                                    notification.BroadCasterUserId,
                                    notification.BroadCasterUserName,
                                    notification.BroadCasterUserLogin,
                                    notification.Message.Text,
                                    notification.Bits);
        }
    }
}