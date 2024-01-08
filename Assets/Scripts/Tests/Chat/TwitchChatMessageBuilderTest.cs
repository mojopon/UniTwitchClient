using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UniTwitchClient.Chat.Models;
using UnityEngine;

namespace UniTwitchClient.Tests.Chat.Models
{
    public class TwitchChatMessageBuilderTest
    {
        [Test]
        public void WithBadgeTest()
        {
            var builder = new TwitchChatMessageBuilder();
            builder.WithBadge("1", "2", "3");
            var result = builder.Build();

            Assert.AreEqual("1", result.Badge.Staff);
            Assert.AreEqual("2", result.Badge.Broadcaster);
            Assert.AreEqual("3", result.Badge.Turbo);
        }

        [Test]
        public void WithColorTest() 
        {
            var builder = new TwitchChatMessageBuilder();
            builder.WithColor("FFFF00");
            var result = builder.Build();

            Assert.AreEqual("FFFF00", result.Color);
        }

        [Test]
        public void WithDisplayNameTest()
        {
            var builder = new TwitchChatMessageBuilder();
            builder.WithDisplayName("TestUser");
            var result = builder.Build();

            Assert.AreEqual("TestUser", result.DisplayName);
        }

        [Test]
        public void WithEmoteOnlyTest()
        {
            var builder = new TwitchChatMessageBuilder();
            builder.WithEmoteOnly("1");
            var result = builder.Build();

            Assert.AreEqual("1", result.EmoteOnly);
        }

        [Test]
        public void WithEmotesTest()
        {
            var builder = new TwitchChatMessageBuilder();
            builder.WithEmote("86", "BibleThump", 0, 9);
            builder.WithEmote("425618", "LUL", 11, 13);
            builder.WithEmote("25", "Kappa", 21, 25);
            var result = builder.Build();

            Assert.AreEqual("86", result.Emotes[0].Id);
            Assert.AreEqual("BibleThump", result.Emotes[0].Name);
            Assert.AreEqual(0, result.Emotes[0].StartIndex);
            Assert.AreEqual(9, result.Emotes[0].EndIndex);
            Assert.AreEqual("425618", result.Emotes[1].Id);
            Assert.AreEqual("LUL", result.Emotes[1].Name);
            Assert.AreEqual(11, result.Emotes[1].StartIndex);
            Assert.AreEqual(13, result.Emotes[1].EndIndex);
            Assert.AreEqual("25", result.Emotes[2].Id);
            Assert.AreEqual("Kappa", result.Emotes[2].Name);
            Assert.AreEqual(21, result.Emotes[2].StartIndex);
            Assert.AreEqual(25, result.Emotes[2].EndIndex);
        }

        [Test]
        public void WithIdTest()
        {
            var builder = new TwitchChatMessageBuilder();
            builder.WithId("c285c9ed-8b1b-4702-ae1c-c64d76cc74ef");
            var result = builder.Build();

            Assert.AreEqual("c285c9ed-8b1b-4702-ae1c-c64d76cc74ef", result.Id);
        }

        [Test]
        public void WithModTest()
        {
            var builder = new TwitchChatMessageBuilder();
            builder.WithMod("1");
            var result = builder.Build();

            Assert.AreEqual("1", result.Mod);
        }

        [Test]
        public void WithRoomIdTest()
        {
            var builder = new TwitchChatMessageBuilder();
            builder.WithRoomId("12345678");
            var result = builder.Build();

            Assert.AreEqual("12345678", result.RoomId);
        }

        [Test]
        public void WithSubscriberTest()
        {
            var builder = new TwitchChatMessageBuilder();
            builder.WithSubscriber("1");
            var result = builder.Build();

            Assert.AreEqual("1", result.Subscriber);
        }

        [Test]
        public void WithTurboTest()
        {
            var builder = new TwitchChatMessageBuilder();
            builder.WithTurbo("1");
            var result = builder.Build();

            Assert.AreEqual("1", result.Turbo);
        }

        [Test]
        public void WithUserIdTest()
        {
            var builder = new TwitchChatMessageBuilder();
            builder.WithUserId("81046256");
            var result = builder.Build();

            Assert.AreEqual("81046256", result.UserId);
        }

        [Test]
        public void WithUserTypeTest()
        {
            var builder = new TwitchChatMessageBuilder();
            builder.WithUserType(" ");
            var result = builder.Build();

            Assert.AreEqual(" ", result.UserType);
        }

        [Test]
        public void WithCapRequestEnabledTest()
        {
            var builder = new TwitchChatMessageBuilder();
            builder.WithCapRequestEnabled(true);
            var result = builder.Build();

            Assert.IsTrue(result.CapRequestEnabled);
        }

        [Test]
        public void WithUserNicknameTest()
        {
            var builder = new TwitchChatMessageBuilder();
            builder.WithUserNickName("TestUserNickName");
            var result = builder.Build();

            Assert.AreEqual("TestUserNickName", result.UserNickname);
        }

        [Test]
        public void WithUserHostTest()
        {
            var builder = new TwitchChatMessageBuilder();
            builder.WithUserHost("petsgomoo@petsgomoo.tmi.twitch.tv");
            var result = builder.Build();

            Assert.AreEqual("petsgomoo@petsgomoo.tmi.twitch.tv", result.UserHost);
        }

        [Test]
        public void WithCommandTest()
        {
            var builder = new TwitchChatMessageBuilder();
            builder.WithCommand("PRIVMSG");
            var result = builder.Build();

            Assert.AreEqual("PRIVMSG", result.Command);
        }

        [Test]
        public void WithChannelTest()
        {
            var builder = new TwitchChatMessageBuilder();
            builder.WithChannel("petsgomoo");
            var result = builder.Build();

            Assert.AreEqual("petsgomoo", result.Channel);
        }

        [Test]
        public void WithMessageTest()
        {
            var builder = new TwitchChatMessageBuilder();
            builder.WithMessage("hello");
            var result = builder.Build();

            Assert.AreEqual("hello", result.Message);
        }
    }
}