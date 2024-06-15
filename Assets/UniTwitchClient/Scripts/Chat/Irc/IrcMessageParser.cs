using System;
using UniTwitchClient.Chat.Models;
using UnityEngine;

namespace UniTwitchClient.Chat
{
    public static class IrcMessageParser
    {
        public static TwitchChatMessage ParseMessage(string message)
        {
            int idx = 0;

            string rawTagsComponent = null;
            string rawSourceComponent = null;
            string rawCommandComponent = null;
            string rawParametersComponent = null;

            TwitchChatMessageBuilder builder = new TwitchChatMessageBuilder();

            int endIdx = 0;

            if (message[idx] == '@')
            {
                endIdx = message.IndexOf(' ');
                rawTagsComponent = message.Substring(1, endIdx);
                idx = endIdx + 1;
            }

            if (message[idx] == ':')
            {
                idx += 1;
                endIdx = message.IndexOf(' ', idx);
                rawSourceComponent = message.Substring(idx, endIdx - idx);
                idx = endIdx + 1;
            }

            endIdx = endIdx = message.IndexOf(':', idx);
            if (endIdx == -1)
            {
                endIdx = message.Length;
            }

            rawCommandComponent = message.Substring(idx, endIdx - idx).Trim();

            if (endIdx != message.Length)
            {
                idx = endIdx + 1;
                rawParametersComponent = message.Substring(idx);
            }

            var command = ParseCommand(builder, rawCommandComponent);
            if (string.IsNullOrEmpty(command))
            {
                return null;
            }
            else
            {
                if (!string.IsNullOrEmpty(rawTagsComponent))
                {
                    ParseTags(builder, rawTagsComponent, rawParametersComponent);
                }

                ParseSource(builder, rawSourceComponent);
                ParseParameters(builder, rawParametersComponent);
            }


            return builder.Build();
        }

        private static string ParseCommand(TwitchChatMessageBuilder builder, string rawCommandComponent)
        {
            if (string.IsNullOrEmpty(rawCommandComponent)) { return null; }

            var commandParts = rawCommandComponent.Split(' ');

            switch (commandParts[0])
            {
                case "JOIN":
                case "PART":
                case "NOTICE":
                case "CLEARCHAT":
                case "HOSTTARGET":
                case "PRIVMSG":
                    {
                        builder.WithCommand(commandParts[0])
                               .WithChannel(commandParts[1]);
                        break;
                    }
                case "PING":
                    {
                        builder.WithCommand(commandParts[0]);
                        break;
                    }
                case "CAP":
                    {
                        builder.WithCommand(commandParts[0]);
                        builder.WithCapRequestEnabled(commandParts[2] == "ACK" ? true : false);
                        break;
                    }
                case "GLOBALUSERSTATE":
                    {
                        builder.WithCommand(commandParts[0]);
                        break;
                    }
                case "USERSTATE":
                case "ROOMSTATE":
                    {
                        builder.WithCommand(commandParts[0])
                               .WithChannel(commandParts[1]);
                        break;
                    }
                case "RECONNECT":
                    {
                        builder.WithCommand(commandParts[0]);
                        break;
                    }
                case "001":
                    {
                        builder.WithCommand(commandParts[0]);
                        break;
                    }
                case "002":
                case "003":
                case "004":
                case "353":
                case "366":
                case "375":
                case "372":
                case "376":
                    {
                        break;
                    }
                default:
                    {
                        Debug.Log($"Unexpected command : {commandParts[0]}");
                        break;
                    }
            }

            return commandParts[0];
        }

        private static void ParseTags(TwitchChatMessageBuilder builder, string rawTagsComponent, string rawParametersComponent)
        {
            var parsedTags = rawTagsComponent.Split(';');

            foreach (var tag in parsedTags)
            {
                var parsedTag = tag.Split('=');
                var tagValue = (string.IsNullOrEmpty(parsedTag[1])) ? null : parsedTag[1];

                switch (parsedTag[0])
                {
                    case "badges":
                    case "badge-info":
                        {
                            if (!string.IsNullOrEmpty(tagValue))
                            {
                                var badges = tagValue.Split(",");
                                string staff = "";
                                string broadcaster = "";
                                string turbo = "";
                                foreach (var badge in badges)
                                {
                                    var badgeParts = badge.Split('/');
                                    switch (badgeParts[0])
                                    {
                                        case "staff":
                                            {
                                                staff = badgeParts[1];
                                                break;
                                            }
                                        case "broadcaster":
                                            {
                                                broadcaster = badgeParts[1];
                                                break;
                                            }
                                        case "turbo":
                                            {
                                                turbo = badgeParts[1];
                                                break;
                                            }
                                    }
                                }

                                builder.WithBadge(staff, broadcaster, turbo);
                            }
                            break;
                        }
                    case "emotes":
                        {
                            if (!string.IsNullOrEmpty(tagValue))
                            {

                                var emotes = tagValue.Split('/');
                                foreach (var emote in emotes)
                                {
                                    var emoteParts = emote.Split(':');
                                    var emoteId = emoteParts[0];
                                    var positions = emoteParts[1].Split(',');
                                    foreach (var position in positions)
                                    {
                                        var positionParts = position.Split('-');
                                        var startPosition = Int32.Parse(positionParts[0]);
                                        var endPosition = Int32.Parse(positionParts[1]);
                                        var emoteName = rawParametersComponent.Substring(startPosition, endPosition + 1 - startPosition);
                                        builder.WithEmote(emoteId, emoteName, startPosition, endPosition);
                                    }
                                }
                            }
                            break;
                        }
                    case "emote-only":
                        {
                            builder.WithEmoteOnly(tagValue);
                            break;
                        }
                    case "display-name":
                        {
                            builder.WithDisplayName(tagValue);
                            break;
                        }
                    case "color":
                        {
                            builder.WithColor(tagValue);
                            break;
                        }
                    case "room-id":
                        {
                            builder.WithRoomId(tagValue);
                            break;
                        }
                    case "user-id":
                        {
                            builder.WithUserId(tagValue);
                            break;
                        }
                }
            }
        }

        private static void ParseSource(TwitchChatMessageBuilder builder, string rawSourceComponent)
        {
            if (string.IsNullOrEmpty(rawSourceComponent))
            {
                return;
            }
            else
            {
                var sourceParts = rawSourceComponent.Split('!');
                if (sourceParts.Length == 2)
                {
                    builder.WithUserNickName(sourceParts[0]);
                    builder.WithUserHost(sourceParts[1]);
                }
            }
        }

        private static void ParseParameters(TwitchChatMessageBuilder builder, string rawParametersComponent)
        {
            if (string.IsNullOrEmpty(rawParametersComponent))
            {
                return;
            }

            builder.WithMessage(rawParametersComponent);

            if (rawParametersComponent[0] == '!')
            {
                var idx = 0;
                var commandParts = rawParametersComponent.Substring(idx + 1).Trim();
                var paramsIdx = commandParts.IndexOf(' ');

                if (paramsIdx == -1)
                {
                    builder.WithBotCommand(commandParts.Substring(0));
                }
                else
                {
                    builder.WithBotCommand(commandParts.Substring(0, paramsIdx));
                    builder.WithBotCommandParams(commandParts.Substring(paramsIdx).Trim());
                }
            }
        }
    }
}
