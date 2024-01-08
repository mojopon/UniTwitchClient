using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace UniTwitchClient.EventSub
{
    public class ChannelFollow
    {
        public string UserId { get; private set; }
        public string UserName { get; private set; }
        public string UserLogin { get; private set; }

        public ChannelFollow(string userId, string userName, string userLogin)
        {
            UserId = userId;
            UserName = userName;
            UserLogin = userLogin;
        }
    }
}