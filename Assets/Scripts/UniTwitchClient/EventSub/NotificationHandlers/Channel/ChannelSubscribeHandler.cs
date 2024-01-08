using System;
using System.Collections;
using System.Collections.Generic;
using UniTwitchClient.EventSub.WebSocket;
using UnityEngine;

namespace UniTwitchClient.EventSub
{
    public class ChannelSubscribeHandler : NotificationHandlerBase<ChannelSubscribe>
    {
        public ChannelSubscribeHandler(Action<ChannelSubscribe> onHandle) : base(onHandle) { }

        public override void HandleNotification(Notification notification)
        {
            var channelSubscribe = new ChannelSubscribe(notification.UserId,
                                                        notification.UserName,
                                                        notification.UserLogin,
                                                        notification.BroadCasterUserId,
                                                        notification.BroadCasterUserName,
                                                        notification.BroadCasterUserLogin,
                                                        notification.Tier,
                                                        notification.IsGift
                                                        );

            _onHandle(channelSubscribe);
        }
    }
}