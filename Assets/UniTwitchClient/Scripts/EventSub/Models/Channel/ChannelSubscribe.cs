namespace UniTwitchClient.EventSub
{
    public class ChannelSubscribe
    {
        public string UserId { get; private set; }
        public string UserName { get; private set; }
        public string UserLogin { get; private set; }
        public string BroadcasterUserId { get; private set; }
        public string BroadcasterUserName { get; private set; }
        public string BroadcasterUserLogin { get; private set; }
        public string Tier { get; private set; }
        public bool IsGift { get; private set; }

        public ChannelSubscribe(string userId, string userName, string userLogin, string broadcasterUserId, string broadcasterUserName, string broadcasterUserLogin, string tier, bool isGift)
        {
            UserId = userId;
            UserName = userName;
            UserLogin = userLogin;
            BroadcasterUserId = broadcasterUserId;
            BroadcasterUserName = broadcasterUserName;
            BroadcasterUserLogin = broadcasterUserLogin;
            Tier = tier;
            IsGift = isGift;
        }
    }
}