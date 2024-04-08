using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniTwitchClient.EventSub.Api.Models
{
    public class Condition
    {
        public string BroadcasterUserId { get; set; }
        public string UserId { get; set; }
        public string ModeratorUserId { get; set; }
        public string FromBroadcasterUserId { get; set; }
        public string ToBroadcasterUserId { get; set; }
    }
}