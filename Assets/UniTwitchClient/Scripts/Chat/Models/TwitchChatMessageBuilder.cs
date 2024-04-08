using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniTwitchClient.Chat.Models
{
    public class TwitchChatMessageBuilder
    {
        private Badge _badge = new Badge("", "", "");
        private string _color;
        private string _displayName;
        private string _emoteOnly;
        private List<Emote> _emotes = new List<Emote>();
        private string _id;
        private string _mod;
        private string _roomId;
        private string _subscriber;
        private string _turbo;
        private string _userId;
        private string _userType;
        private bool _capRequestEnabled;

        private string _userNickname;
        private string _userHost;

        private string _command;
        private string _channel;
        private string _botCommand;
        private string _botCommandParams;

        private string _message;

        public TwitchChatMessage Build()
        {
            return new TwitchChatMessage(_badge,
                                         _color,
                                         _displayName,
                                         _emoteOnly,
                                         _emotes,
                                         _id,
                                         _mod,
                                         _roomId,
                                         _subscriber,
                                         _turbo,
                                         _userId,
                                         _userType,
                                         _capRequestEnabled,
                                         _userNickname,
                                         _userHost,
                                         _command,
                                         _channel,
                                         _botCommand,
                                         _botCommandParams,
                                         _message);
        }

        public TwitchChatMessageBuilder WithBadge(string staff, string broadcaster, string turbo)
        {
            _badge = new Badge(staff, broadcaster, turbo);
            return this;
        }

        public TwitchChatMessageBuilder WithColor(string color)
        {
            _color = color;
            return this;
        }

        public TwitchChatMessageBuilder WithDisplayName(string displayName)
        {
            _displayName = displayName;
            return this;
        }

        public TwitchChatMessageBuilder WithEmoteOnly(string emoteOnly)
        {
            _emoteOnly = emoteOnly;
            return this;
        }

        public TwitchChatMessageBuilder WithEmote(string id, string name, int startIndex, int endIndex)
        {
            var emote = new Emote(id, name, startIndex, endIndex);
            _emotes.Add(emote);
            return this;
        }

        public TwitchChatMessageBuilder WithId(string id)
        {
            _id = id;
            return this;
        }

        public TwitchChatMessageBuilder WithMod(string mod)
        {
            _mod = mod;
            return this;
        }

        public TwitchChatMessageBuilder WithRoomId(string roomId)
        {
            _roomId = roomId;
            return this;
        }

        public TwitchChatMessageBuilder WithSubscriber(string subscriber)
        {
            _subscriber = subscriber;
            return this;
        }

        public TwitchChatMessageBuilder WithTurbo(string turbo)
        {
            _turbo = turbo;
            return this;
        }

        public TwitchChatMessageBuilder WithUserId(string userId)
        {
            _userId = userId;
            return this;
        }

        public TwitchChatMessageBuilder WithUserType(string userType)
        {
            _userType = userType;
            return this;
        }

        public TwitchChatMessageBuilder WithCapRequestEnabled(bool flag)
        {
            _capRequestEnabled = flag;
            return this;
        }

        public TwitchChatMessageBuilder WithUserNickName(string userNickname)
        {
            _userNickname = userNickname;
            return this;
        }

        public TwitchChatMessageBuilder WithUserHost(string userHost)
        {
            _userHost = userHost;
            return this;
        }

        public TwitchChatMessageBuilder WithCommand(string command)
        {
            _command = command;
            return this;
        }

        public TwitchChatMessageBuilder WithChannel(string channel)
        {
            _channel = channel;
            return this;
        }

        public TwitchChatMessageBuilder WithBotCommand(string botCommand) 
        {
            _botCommand = botCommand;
            return this;
        }

        public TwitchChatMessageBuilder WithBotCommandParams(string botCommandParams)
        {
            _botCommandParams = botCommandParams;
            return this;
        }

        public TwitchChatMessageBuilder WithMessage(string message)
        {
            _message = message;
            return this;
        }
    }
}