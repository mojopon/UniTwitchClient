using System.Collections;
using System.Collections.Generic;
using UniTwitchClient.EventSub.WebSocket;
using UnityEngine;

namespace UniTwitchClient.EventSub
{
    public class ChannelFollowConverter : INotificationConverter
    {
        public object Convert(Notification notification)
        {
            return new ChannelFollow(notification.UserId,
                                     notification.UserName,
                                     notification.UserLogin,
                                     notification.BroadCasterUserId,
                                     notification.BroadCasterUserName,
                                     notification.BroadCasterUserLogin,
                                     notification.FollowedAt);
        }
    }
}
