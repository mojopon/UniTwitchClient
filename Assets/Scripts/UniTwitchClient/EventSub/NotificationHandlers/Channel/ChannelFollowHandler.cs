using System;
using System.Collections;
using System.Collections.Generic;
using UniTwitchClient.EventSub.WebSocket;
using UnityEngine;

namespace UniTwitchClient.EventSub
{
    public class ChannelFollowHandler : NotificationHandlerBase<ChannelFollow>
    {
        public ChannelFollowHandler(Action<ChannelFollow> onHandle) : base(onHandle) { }

        public override void HandleNotification(Notification notification)
        {
            var channelFollow = new ChannelFollow(notification.UserId,
                                                  notification.UserName,
                                                  notification.UserLogin);

            _onHandle(channelFollow);
        }
    }
}