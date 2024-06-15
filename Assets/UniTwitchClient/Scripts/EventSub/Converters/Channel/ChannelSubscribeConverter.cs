using UniTwitchClient.EventSub.WebSocket;

namespace UniTwitchClient.EventSub.Converters
{
    public class ChannelSubscribeConverter : INotificationConverter
    {
        public object Convert(Notification notification)
        {
            return new ChannelSubscribe(notification.UserId,
                                        notification.UserName,
                                        notification.UserLogin,
                                        notification.BroadCasterUserId,
                                        notification.BroadCasterUserName,
                                        notification.BroadCasterUserLogin,
                                        notification.Tier,
                                        notification.IsGift
                                        );
        }
    }
}