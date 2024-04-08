using System;
using System.Collections;
using System.Collections.Generic;
using UniTwitchClient.EventSub.WebSocket;
using UnityEngine;

namespace UniTwitchClient.EventSub
{
    public class ChannelPointsCustomRewardRedemptionAddHandler : NotificationHandlerBase<ChannelPointsCustomRewardRedemptionAdd>
    {
        public ChannelPointsCustomRewardRedemptionAddHandler(Action<ChannelPointsCustomRewardRedemptionAdd> onHandle) : base(onHandle) { }

        public override void HandleNotification(Notification notification)
        {
            var channelPointsCustomRewardRedemptionAdd = new ChannelPointsCustomRewardRedemptionAdd(notification.UserId,
                                                                                                    notification.UserName,
                                                                                                    notification.UserLogin,
                                                                                                    notification.BroadCasterUserId,
                                                                                                    notification.BroadCasterUserName,
                                                                                                    notification.BroadCasterUserLogin,
                                                                                                    null,
                                                                                                    notification.RewardTitle,
                                                                                                    notification.RewardCost,
                                                                                                    notification.RewardPrompt,
                                                                                                    notification.RedeemedAt);

            _onHandle(channelPointsCustomRewardRedemptionAdd);
        }
    }
}
