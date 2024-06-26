using NUnit.Framework;
using System.Runtime.InteropServices;
using UniTwitchClient.Chat;
using UniTwitchClient.Chat.Models;

namespace UniTwitchClient.Tests.Chat.Models
{
    public class IrcMessageParserTest
    {
        private readonly string message = "@badges=staff/1,broadcaster/1,turbo/1;color=#FF0000;display-name=PetsgomOO;emote-only=1;emotes=33:0-7;flags=0-7:A.6/P.6,25-36:A.1/I.2;id=c285c9ed-8b1b-4702-ae1c-c64d76cc74ef;mod=0;room-id=81046256;subscriber=0;turbo=0;tmi-sent-ts=1550868292494;user-id=81046256;user-type=staff :petsgomoo!petsgomoo@petsgomoo.tmi.twitch.tv PRIVMSG #petsgomoo :DansGame";
        private readonly string messageWithEmotes = "@badge-info=;badges=;color=;display-name=mojomojopon;emotes=86:0-9/425618:11-13/25:21-25;first-msg=1;flags=;id=2c56c69d-8e82-4e31-a152-c48cadfbf5cc;mod=0;returning-chatter=0;room-id=186693621;subscriber=0;tmi-sent-ts=1703404713992;turbo=0;user-id=992276336;user-type= :mojomojopon!mojomojopon@mojomojopon.tmi.twitch.tv PRIVMSG #anomaloris :BibleThump LUL hello Kappa";
        private readonly string messageWithDuplicatedEmotes = "@badge-info=;badges=;color=;display-name=mojomojopon;emotes=86:0-9,34-43/425618:11-13;first-msg=0;flags=;id=c51cce13-08c0-4a6d-8bb7-4243b4356225;mod=0;returning-chatter=0;room-id=186693621;subscriber=0;tmi-sent-ts=1704433200401;turbo=0;user-id=992276336;user-type= :mojomojopon!mojomojopon@mojomojopon.tmi.twitch.tv PRIVMSG #anomaloris :BibleThump LUL message with emote BibleThump";
        private readonly string messageWithBotCommand = ":lovingt3s!lovingt3s@lovingt3s.tmi.twitch.tv PRIVMSG #lovingt3s :!dilly";
        private readonly string messageWithBotCommandWithParams = ":lovingt3s!lovingt3s@lovingt3s.tmi.twitch.tv PRIVMSG #lovingt3s :!botcommand params";
        private readonly string messageWithPing = "PING :tmi.twitch.tv";
        private readonly string messageOnLogged = ":tmi.twitch.tv 001 testuser :Welcome, GLHF!";

        [Test]
        public void ParseMessageTest()
        {
            var result = IrcMessageParser.ParseMessage(message);

            Assert.AreEqual("1", result.Badge.Staff);
            Assert.AreEqual("1", result.Badge.Broadcaster);
            Assert.AreEqual("1", result.Badge.Turbo);

            Assert.AreEqual("PetsgomOO", result.DisplayName);

            Assert.AreEqual("petsgomoo", result.UserNickname);
            Assert.AreEqual("petsgomoo@petsgomoo.tmi.twitch.tv", result.UserHost);

            Assert.AreEqual(TwitchIrcCommand.PrivMsg, result.Command);
        }

        [Test]
        public void ParseMessageWithEmotesTest() 
        {
            var result = IrcMessageParser.ParseMessage(messageWithEmotes);

            Assert.AreEqual("BibleThump", result.Emotes[0].Name);
            Assert.AreEqual("86", result.Emotes[0].Id);
            Assert.AreEqual(0, result.Emotes[0].StartIndex);
            Assert.AreEqual(9, result.Emotes[0].EndIndex);

            Assert.AreEqual("LUL", result.Emotes[1].Name);
            Assert.AreEqual("425618", result.Emotes[1].Id);
            Assert.AreEqual(11, result.Emotes[1].StartIndex);
            Assert.AreEqual(13, result.Emotes[1].EndIndex);

            Assert.AreEqual("Kappa", result.Emotes[2].Name);
            Assert.AreEqual("25", result.Emotes[2].Id);
            Assert.AreEqual(21, result.Emotes[2].StartIndex);
            Assert.AreEqual(25, result.Emotes[2].EndIndex);

            Assert.AreEqual(TwitchIrcCommand.PrivMsg, result.Command);
        }

        [Test]
        public void ParseMessageWithDuplicatedEmotesTest() 
        {
            var result = IrcMessageParser.ParseMessage(messageWithDuplicatedEmotes);

            Assert.AreEqual("BibleThump", result.Emotes[0].Name);
            Assert.AreEqual("86", result.Emotes[0].Id);
            Assert.AreEqual(0, result.Emotes[0].StartIndex);
            Assert.AreEqual(9, result.Emotes[0].EndIndex);

            Assert.AreEqual("BibleThump", result.Emotes[1].Name);
            Assert.AreEqual("86", result.Emotes[1].Id);
            Assert.AreEqual(34, result.Emotes[1].StartIndex);
            Assert.AreEqual(43, result.Emotes[1].EndIndex);

            Assert.AreEqual("LUL", result.Emotes[2].Name);
            Assert.AreEqual("425618", result.Emotes[2].Id);
            Assert.AreEqual(11, result.Emotes[2].StartIndex);
            Assert.AreEqual(13, result.Emotes[2].EndIndex);

            Assert.AreEqual(TwitchIrcCommand.PrivMsg, result.Command);
        }

        [Test]
        public void ParseMessageWithBotCommandTest() 
        {
            var result = IrcMessageParser.ParseMessage(messageWithBotCommand);
            Assert.AreEqual("dilly", result.BotCommand);

            Assert.AreEqual(TwitchIrcCommand.PrivMsg, result.Command);
        }

        [Test]
        public void ParseMessageWithBotCommandWithParamsTest()
        {
            var result = IrcMessageParser.ParseMessage(messageWithBotCommandWithParams);
            Assert.AreEqual("botcommand", result.BotCommand);
            Assert.AreEqual("params", result.BotCommandParams);

            Assert.AreEqual(TwitchIrcCommand.PrivMsg, result.Command);
        }

        [Test]
        public void ParseMessageWithPingTest() 
        {
            var result = IrcMessageParser.ParseMessage(messageWithPing);

            Assert.AreEqual(TwitchIrcCommand.Ping, result.Command);
            Assert.AreEqual("PING", result.CommandRaw);
        }

        [Test]
        public void ParseMessageOnLogged() 
        {
            var result = IrcMessageParser.ParseMessage(messageOnLogged);

            Assert.AreEqual(TwitchIrcCommand.Numeric001, result.Command);
            Assert.AreEqual("001", result.CommandRaw);
        }
    }
}