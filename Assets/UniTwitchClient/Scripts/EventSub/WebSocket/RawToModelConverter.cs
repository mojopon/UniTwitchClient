using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Xml;
using UniTwitchClient.EventSub.WebSocket;
using UniTwitchClient.EventSub.WebSocket.Models.Raws;
using UnityEngine;

namespace UniTwitchClient.EventSub.WebSocket
{
    public static class RawToModelConverter
    {
        public static Welcome ConvertRawToModel(this welcome_raw source)
        {
            Welcome welcomeMessage = new Welcome();
            if (source.metadata != null)
            {
                welcomeMessage.MessageType = source.metadata.message_type;
            }
            if (source.payload != null && source.payload.session != null)
            {
                welcomeMessage.SessionId = source.payload.session.id;
                welcomeMessage.KeepAliveTimeoutSeconds = source.payload.session.keepalive_timeout_seconds;
            }
            return welcomeMessage;
        }

        public static WebSocketMessageBase ConvertRawToModel(this message_base source)
        {
            WebSocketMessageBase messageBase = new WebSocketMessageBase();
            if (source.metadata != null)
            {
                messageBase.MessageType = source.metadata.message_type;
            }
            return messageBase;
        }

        public static KeepAlive ConvertRawToModel(this keepalive_raw source)
        {
            KeepAlive keepaliveMessage = new KeepAlive();
            if (source.metadata != null)
            {
                keepaliveMessage.MessageType = source.metadata.message_type;
            }
            return keepaliveMessage;
        }

        public static Notification ConvertRawToModel(this notification_raw source)
        {
            Notification notification = new Notification();

            if (source.metadata != null)
            {
                notification.MessageType = source.metadata.message_type;
                notification.MessageTimeStamp = ParseDateTime(source.metadata.message_timestamp);
            }

            if (source.payload != null)
            {
                notification.SubscriptionType = SubscriptionTypeConverter.ToSubscriptionType(source.payload.subscription.type);
            }

            @event eventSource = null;
            if (source.payload != null)
            {
                eventSource = source.payload.@event;
            }

            if (eventSource != null)
            {

                notification.UserId = eventSource.user_id;
                notification.UserName = eventSource.user_name;
                notification.UserLogin = eventSource.user_login;
                notification.BroadCasterUserId = eventSource.broadcaster_user_id;
                notification.BroadCasterUserName = eventSource.broadcaster_user_name;
                notification.BroadCasterUserLogin = eventSource.broadcaster_user_login;
                notification.FollowedAt = ParseDateTime(eventSource.followed_at);
                notification.CumulativeMonths = eventSource.cumulative_months;
                notification.StreakMonths = eventSource.streak_months;
                notification.DurationMonths = eventSource.duration_months;

                if (eventSource.reward != null)
                {
                    notification.RewardId = eventSource.reward.id;
                    notification.RewardTitle = eventSource.reward.title;
                    notification.RewardPrompt = eventSource.reward.prompt;
                    notification.RewardCost = eventSource.reward.cost;
                }

                if (eventSource.message != null) 
                {
                    notification.Message = new Message()
                    {
                        Text = eventSource.message.text
                    };

                    if (eventSource.message.emotes != null)
                    {
                        List<Emote> emotes = new List<Emote>();
                        foreach (var emote in eventSource.message.emotes)
                        {
                            emotes.Add(new Emote()
                            {
                                Begin = emote.begin,
                                End = emote.end,
                                Id = emote.id,
                            });
                        }

                        notification.Message.Emotes = emotes;
                    }
                }

                notification.RedeemedAt = ParseDateTime(eventSource.redeemed_at);

                notification.IsGift = eventSource.is_gift;
                notification.Tier = eventSource.tier;
            }

            return notification;
        }

        private static DateTime ParseDateTime(string dateTimeRaw) 
        {
            DateTime dateTime = new DateTime();

            try
            {
                dateTime = XmlConvert.ToDateTime(dateTimeRaw, XmlDateTimeSerializationMode.Utc);
            }catch (Exception ex) { }

            return dateTime;
        }
    }
}