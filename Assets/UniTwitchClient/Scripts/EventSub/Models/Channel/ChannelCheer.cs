namespace UniTwitchClient.EventSub
{
    public class ChannelCheer
    {
        public bool IsAnonymous { get; private set; }
        public string UserId { get; private set; }
        public string UserName { get; private set; }
        public string UserLogin { get; private set; }
        public string BroadcasterUserId { get; private set; }
        public string BroadcasterUserLogin { get; private set; }
        public string BroadcasterUserName { get; private set; }
        public string Message { get; private set; }
        public int Bits { get; private set; }

        public ChannelCheer(bool isAnonymous, string userId, string userName, string userLogin, string broadcasterUserId, string broadcasterUserName, string broadcasterUserLogin, string message, int bits)
        {
            IsAnonymous = isAnonymous;
            UserId = userId;
            UserName = userName;
            UserLogin = userLogin;
            BroadcasterUserId = broadcasterUserId;
            BroadcasterUserName = broadcasterUserName;
            BroadcasterUserLogin = broadcasterUserLogin;
            Message = message;
            Bits = bits;
        }
    }
}