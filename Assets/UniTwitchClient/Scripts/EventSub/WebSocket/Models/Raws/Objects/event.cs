using System;

namespace UniTwitchClient.EventSub.WebSocket.Models.Raws
{
    // Event Properties Reference
    // https://dev.twitch.tv/docs/eventsub/eventsub-reference/

    [Serializable]
    public class @event
    {
        public string user_id;
        public string user_login;
        public string user_name;
        public string broadcaster_user_id;
        public string broadcaster_user_login;
        public string broadcaster_user_name;

        public bool is_anonymous;

        public message message;

        public reward reward;

        // Channel Cheer Event
        public int bits;

        // Channel Subscription Message Event
        public int cumulative_months;
        public int streak_months;
        public int duration_months;

        // Channel Follow Event
        public string followed_at;

        // Channel Subscribe Event
        public string tier;
        public bool is_gift;

        // Channel Points Custom Reward Redemption Add Event
        public string redeemed_at;
    }
}
