namespace UniTwitchClient.Chat.Models
{
    public static class TwitchIrcCommandConverter
    {
        public static TwitchIrcCommand CommandStringToCommandEnum(string str)
        {
            switch (str)
            {
                case "JOIN":
                    return TwitchIrcCommand.Join;
                case "PART":
                    return TwitchIrcCommand.Part;
                case "NOTICE":
                    return TwitchIrcCommand.Notice;
                case "CLEARCHAT":
                    return TwitchIrcCommand.ClearChat;
                case "HOSTTARGET":
                    return TwitchIrcCommand.HostTarget;
                case "PRIVMSG":
                    return TwitchIrcCommand.PrivMsg;
                case "PING":
                    return TwitchIrcCommand.Ping;
                case "CAP":
                    return TwitchIrcCommand.Cap;
                case "GLOBALUSERSTATE":
                    return TwitchIrcCommand.GlobalUserState;
                case "USERSTATE":
                    return TwitchIrcCommand.UserState;
                case "ROOMSTATE":
                    return TwitchIrcCommand.RoomState;
                case "RECONNECT":
                    return TwitchIrcCommand.Reconnect;
                case "001":
                    return TwitchIrcCommand.Numeric001;
                default:
                    return TwitchIrcCommand.Default;
            }
        }
    }
}