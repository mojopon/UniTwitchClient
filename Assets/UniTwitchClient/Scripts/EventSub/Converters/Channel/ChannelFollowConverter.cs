using UniTwitchClient.EventSub.WebSocket;

namespace UniTwitchClient.EventSub.Converters
{
    public class ChannelFollowConverter : INotificationConverter
    {
        public object Convert(Notification notification)
        {
            return new ChannelFollow(notification.UserId,
                                     notification.UserName,
                                     notification.UserLogin,
                                     notification.BroadCasterUserId,
                                     notification.BroadCasterUserName,
                                     notification.BroadCasterUserLogin,
                                     notification.FollowedAt);
        }
    }
}
