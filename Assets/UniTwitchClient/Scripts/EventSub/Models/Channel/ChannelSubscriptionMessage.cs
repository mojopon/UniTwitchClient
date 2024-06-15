using UniTwitchClient.EventSub.WebSocket;

namespace UniTwitchClient.EventSub
{
    public class ChannelSubscriptionMessage
    {
        public string UserId { get; private set; }
        public string UserName { get; private set; }
        public string UserLogin { get; private set; }
        public string BroadcasterUserId { get; private set; }
        public string BroadcasterUserName { get; private set; }
        public string BroadcasterUserLogin { get; private set; }
        public string Tier { get; private set; }
        public Message Message { get; private set; }
        public int CumulativeMonths { get; private set; }
        public int StreakMonths { get; private set; }
        public int DurationMonths { get; private set; }

        public ChannelSubscriptionMessage(string userId, string userName, string userLogin, string broadcasterUserId, string broadcasterUserName, string broadcasterUserLogin, string tier, Message message, int cumulative_months, int streak_months, int duration_months)
        {
            UserId = userId;
            UserName = userName;
            UserLogin = userLogin;
            BroadcasterUserId = broadcasterUserId;
            BroadcasterUserName = broadcasterUserName;
            BroadcasterUserLogin = broadcasterUserLogin;
            Tier = tier;
            Message = message;
            CumulativeMonths = cumulative_months;
            StreakMonths = streak_months;
            DurationMonths = duration_months;
        }
    }
}