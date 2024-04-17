using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniTwitchClient.EventSub.WebSocket
{
    public class Notification
    {
        public string MessageType { get; set; }
        public DateTime MessageTimeStamp { get; set; }
        public SubscriptionType SubscriptionType { get; set; }

        public Message Message { get; set; }

        public string BroadCasterUserId { get; set; }
        public string BroadCasterUserLogin { get; set; }
        public string BroadCasterUserName { get; set; }
        public string UserId { get; set; }
        public string UserLogin { get; set; }
        public string UserName { get; set; }

        // Channel Subscription Message Event
        public int CumulativeMonths { get; set; }
        public int StreakMonths { get; set; }
        public int DurationMonths { get; set; }

        // Channel Follow Event
        public DateTime FollowedAt { get; set; }

        // Reward
        public string RewardId { get; set; }
        public string RewardTitle { get; set; }
        public int RewardCost { get; set; }
        public string RewardPrompt { get; set; }

        // Channel Subscribe Event
        public string Tier { get; set; }
        public bool IsGift { get; set; }

        // Channel Points Custom Reward Redemption Add Event
        public string Status { get; set; }
        public DateTime RedeemedAt { get; set; }
    }
}