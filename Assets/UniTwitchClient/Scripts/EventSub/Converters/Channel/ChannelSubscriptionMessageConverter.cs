using UniTwitchClient.EventSub.WebSocket;

namespace UniTwitchClient.EventSub.Converters
{
    public class ChannelSubscriptionMessageConverter : INotificationConverter
    {
        public object Convert(Notification notification)
        {
            return new ChannelSubscriptionMessage(notification.UserId,
                                                  notification.UserName,
                                                  notification.UserLogin,
                                                  notification.BroadCasterUserId,
                                                  notification.BroadCasterUserName,
                                                  notification.BroadCasterUserLogin,
                                                  notification.Tier,
                                                  notification.Message,
                                                  notification.CumulativeMonths,
                                                  notification.StreakMonths,
                                                  notification.DurationMonths
                                                  );
        }
    }
}
