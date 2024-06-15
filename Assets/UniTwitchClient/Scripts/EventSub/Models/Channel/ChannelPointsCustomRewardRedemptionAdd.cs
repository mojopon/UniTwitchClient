using System;

namespace UniTwitchClient.EventSub
{
    public class ChannelPointsCustomRewardRedemptionAdd
    {
        public string UserId { get; private set; }
        public string UserName { get; private set; }
        public string UserLogin { get; private set; }
        public string BroadcasterUserId { get; private set; }
        public string BroadcasterUserName { get; private set; }
        public string BroadcasterUserLogin { get; private set; }

        public string Status { get; private set; }
        public string RewardTitle { get; private set; }
        public int RewardCost { get; private set; }
        public string RewardPrompt { get; private set; }

        public DateTime RedeemedAt { get; private set; }

        public ChannelPointsCustomRewardRedemptionAdd(string userId, string userName, string userLogin, string broadcasterUserId, string broadcasterUserName, string broadcasterUserLogin, string status, string rewardTitle, int rewardCost, string rewardPrompt, DateTime redeemedAt)
        {
            UserId = userId;
            UserName = userName;
            UserLogin = userLogin;
            BroadcasterUserId = broadcasterUserId;
            BroadcasterUserLogin = broadcasterUserLogin;
            BroadcasterUserName = broadcasterUserName;
            Status = status;
            RewardTitle = rewardTitle;
            RewardCost = rewardCost;
            RewardPrompt = rewardPrompt;
            RedeemedAt = redeemedAt;
        }
    }
}