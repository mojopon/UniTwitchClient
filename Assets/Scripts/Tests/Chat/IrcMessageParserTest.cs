using NUnit.Framework;
using UniTwitchClient.Chat;

namespace UniTwitchClient.Tests.Chat.Models
{
    public class IrcMessageParserTest
    {
        private string messageWithEmotes = "@badge-info=;badges=;color=;display-name=mojomojopon;emotes=86:0-9/425618:11-13/25:21-25;first-msg=1;flags=;id=2c56c69d-8e82-4e31-a152-c48cadfbf5cc;mod=0;returning-chatter=0;room-id=186693621;subscriber=0;tmi-sent-ts=1703404713992;turbo=0;user-id=992276336;user-type= :mojomojopon!mojomojopon@mojomojopon.tmi.twitch.tv PRIVMSG #anomaloris :BibleThump LUL hello Kappa";
        private string messageWithDuplicatedEmotes = "@badge-info=;badges=;color=;display-name=mojomojopon;emotes=86:0-9,34-43/425618:11-13;first-msg=0;flags=;id=c51cce13-08c0-4a6d-8bb7-4243b4356225;mod=0;returning-chatter=0;room-id=186693621;subscriber=0;tmi-sent-ts=1704433200401;turbo=0;user-id=992276336;user-type= :mojomojopon!mojomojopon@mojomojopon.tmi.twitch.tv PRIVMSG #anomaloris :BibleThump LUL message with emote BibleThump";
        private string message = "@badges=staff/1,broadcaster/1,turbo/1;color=#FF0000;display-name=PetsgomOO;emote-only=1;emotes=33:0-7;flags=0-7:A.6/P.6,25-36:A.1/I.2;id=c285c9ed-8b1b-4702-ae1c-c64d76cc74ef;mod=0;room-id=81046256;subscriber=0;turbo=0;tmi-sent-ts=1550868292494;user-id=81046256;user-type=staff :petsgomoo!petsgomoo@petsgomoo.tmi.twitch.tv PRIVMSG #petsgomoo :DansGame";

        [Test]
        public void ParseMessageTest()
        {
            var result = IrcMessageParser.ParseMessage(message);

            Assert.AreEqual("1", result.Badge.Staff);
            Assert.AreEqual("1", result.Badge.Broadcaster);
            Assert.AreEqual("1", result.Badge.Turbo);

            Assert.AreEqual("PetsgomOO", result.DisplayName);
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
        }
    }
}