using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniTwitchClient.Chat.Models
{
    public class TwitchChatMessage
    {
        public Badge Badge { get; }
        public string Color { get; }
        public string DisplayName { get; }
        public string EmoteOnly { get; }
        public IReadOnlyList<Emote> Emotes { get; }
        public string Id { get; }
        public string Mod { get; }
        public string RoomId { get; }
        public string Subscriber { get; }
        public string Turbo { get; }
        public string UserId { get; }
        public string UserType { get; }
        public bool CapRequestEnabled { get; }

        public string UserNickname { get; }
        public string UserHost { get; }

        public string Command { get; }
        public string Channel { get; }

        public string Message { get; }

        public TwitchChatMessage(Badge badge,
                                 string color,
                                 string displayName,
                                 string emoteOnly,
                                 IReadOnlyList<Emote> emotes,
                                 string id,
                                 string mod,
                                 string roomId,
                                 string subscriber,
                                 string turbo,
                                 string userId,
                                 string userType,
                                 bool capRequestEnabled,
                                 string userNickname,
                                 string userHost,
                                 string command,
                                 string channel,
                                 string message)
        {
            Badge = badge;
            Color = color;
            DisplayName = displayName;
            EmoteOnly = emoteOnly;
            Emotes = emotes;
            Id = id;
            Mod = mod;
            RoomId = roomId;
            Subscriber = subscriber;
            Turbo = turbo;
            UserId = userId;
            UserType = userType;
            CapRequestEnabled = capRequestEnabled;
            UserNickname = userNickname;
            UserHost = userHost;
            Command = command;
            Channel = channel;
            Message = message;
        }
    }
}
