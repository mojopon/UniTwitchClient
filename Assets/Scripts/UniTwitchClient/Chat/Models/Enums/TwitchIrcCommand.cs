using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniTwitchClient.Chat.Models
{
    public enum TwitchIrcCommand
    {
        Default = 0,
        Join,
        Part,
        Notice,
        ClearChat,
        HostTarget,
        PrivMsg,
        Ping,
        Cap,
        GlobalUserState,
        UserState,
        RoomState,
        Reconnect,
        Numeric001, // This means Logged in (successfully authenticated)
    }
}