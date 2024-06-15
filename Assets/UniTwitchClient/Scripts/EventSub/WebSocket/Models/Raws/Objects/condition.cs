using System;

namespace UniTwitchClient.EventSub.WebSocket.Models.Raws
{
    [Serializable]
    public class condition
    {
        public string broadcaster_user_id;
        public string user_id;
        public string moderator_user_id;
        public string reward_id;
    }
}
