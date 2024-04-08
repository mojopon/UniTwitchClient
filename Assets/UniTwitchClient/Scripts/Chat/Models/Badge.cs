using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniTwitchClient.Chat.Models
{
    public class Badge
    {
        public string Staff { get; }
        public string Broadcaster { get; }
        public string Turbo { get; }

        public Badge(string staff, string broadcaster, string turbo)
        {
            Staff = staff;
            Broadcaster = broadcaster;
            Turbo = turbo;
        }
    }
}
