using System.Collections;
using System.Collections.Generic;
using UniTwitchClient.EventSub.WebSocket;
using UnityEngine;

namespace UniTwitchClient.EventSub.Converters
{
    public class ChannelPointsCustomRewardRedemptionAddConverter : INotificationConverter
    {
        public object Convert(Notification notification)
        {
            return new ChannelPointsCustomRewardRedemptionAdd(notification.UserId,
                                                              notification.UserName,
                                                              notification.UserLogin,
                                                              notification.BroadCasterUserId,
                                                              notification.BroadCasterUserName,
                                                              notification.BroadCasterUserLogin,
                                                              notification.Status,
                                                              notification.RewardTitle,
                                                              notification.RewardCost,
                                                              notification.RewardPrompt,
                                                              notification.RedeemedAt);
        }
    }
}