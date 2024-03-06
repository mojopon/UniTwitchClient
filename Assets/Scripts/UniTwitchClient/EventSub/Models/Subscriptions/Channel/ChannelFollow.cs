using System;
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
        public string BroadcasterUserId { get; private set; }
        public string BroadcasterUserLogin { get; private set; }
        public string BroadcasterUserName { get; private set; }
        public DateTime FollowedAt { get; private set; }


        public ChannelFollow(string userId, string userName, string userLogin, string broadcasterUserId, string broadcasterUserName, string broadcasterUserLogin, DateTime followedAt)
        {
            UserId = userId;
            UserName = userName;
            UserLogin = userLogin;
            BroadcasterUserId = broadcasterUserId;
            BroadcasterUserName = broadcasterUserName;
            BroadcasterUserLogin = broadcasterUserLogin;
            FollowedAt = followedAt;
        }
    }
}