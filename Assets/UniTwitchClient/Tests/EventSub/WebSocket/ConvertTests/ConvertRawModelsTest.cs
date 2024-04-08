using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UniTwitchClient.EventSub;
using UniTwitchClient.EventSub.WebSocket.Models.Raws;
using UnityEngine;

namespace UniTwitchClient.Tests.EventSub.WebSocket
{
    public class ConvertRawModelsTest
    {
        [Test]
        public void ConvertConditionTest() 
        {
            var data = "{\n  \"broadcaster_user_id\": \"12345\",\n  \"user_id\": \"23456\",\n  \"moderator_user_id\": \"34567\",\n  \"reward_id\": \"45678\"\n}";
            condition model = null;
            model = JsonWrapper.ConvertFromJson<condition>(data);

            Assert.IsNotNull(model);
            Assert.AreEqual("12345", model.broadcaster_user_id);
            Assert.AreEqual("23456", model.user_id);
            Assert.AreEqual("34567", model.moderator_user_id);
            Assert.AreEqual("45678", model.reward_id);
        }

        [Test]
        public void ConvertConditionFromBlankDataTest() 
        {
            var data = "{}";
            condition model = null;
            model = JsonWrapper.ConvertFromJson<condition>(data);

            Assert.IsNotNull(model);
            Assert.IsNull(model.broadcaster_user_id);
            Assert.IsNull(model.user_id);
            Assert.IsNull(model.moderator_user_id);
            Assert.IsNull(model.reward_id);
        }

        [Test]
        public void ConvertEventTest() 
        {
            var data = "{\n  \"user_id\": \"12345\",\n  \"user_login\": \"testUser\",\n  \"user_name\": \"testUserName\",\n  \"broadcaster_user_id\": \"23456\",\n  \"broadcaster_user_login\": \"broadCaster\",\n  \"broadcaster_user_name\": \"broadCasterName\",\n  \"followed_at\": \"1234\",\n  \"reward\": {\n    \"id\": \"23456\",\n    \"title\": \"title\",\n    \"cost\": 123,\n    \"prompt\": \"prompt\"\n  },\n  \"tier\": \"11111\",\n  \"is_gift\": true\n}";
            @event model = null;
            model = JsonWrapper.ConvertFromJson<@event>(data);

            Assert.IsNotNull(model);
            Assert.AreEqual("12345", model.user_id);
            Assert.AreEqual("testUser", model.user_login);
            Assert.AreEqual("testUserName", model.user_name);
            Assert.AreEqual("23456", model.broadcaster_user_id);
            Assert.AreEqual("broadCaster", model.broadcaster_user_login);
            Assert.AreEqual("broadCasterName", model.broadcaster_user_name);
            Assert.AreEqual("1234", model.followed_at);

            Assert.AreEqual("23456", model.reward.id);
            Assert.AreEqual("title", model.reward.title);
            Assert.AreEqual(123, model.reward.cost);
            Assert.AreEqual("prompt", model.reward.prompt);


            Assert.AreEqual("11111", model.tier);
            Assert.AreEqual(true, model.is_gift);
        }

        [Test]
        public void ConvertEventFromBlankDataTest() 
        {
            var data = "{}";
            @event model = null;
            model = JsonWrapper.ConvertFromJson<@event>(data);

            Assert.IsNotNull(model);
            Assert.IsNull(model.user_id);
            Assert.IsNull(model.user_login);
            Assert.IsNull(model.user_name);
            Assert.IsNull(model.broadcaster_user_id);
            Assert.IsNull(model.broadcaster_user_login);
            Assert.IsNull(model.broadcaster_user_name);
            Assert.IsNull(model.followed_at);
            Assert.IsNull(model.tier);
            Assert.IsFalse(model.is_gift);

            Assert.IsNull(model.reward);
        }

        [Test]
        public void ConvertMetaDataTest() 
        {
            var data = "{\n  \"message_id\": \"12345\",\n  \"message_type\": \"type\",\n  \"message_timestamp\": \"timestamp\",\n  \"subscription_type\": \"subscriptionType\",\n  \"subscription_version\": \"subscriptionVersion\"\n}";
            metadata model = null;
            model = JsonWrapper.ConvertFromJson<metadata>(data);

            Assert.IsNotNull(model);
            Assert.AreEqual("12345", model.message_id);
            Assert.AreEqual("type", model.message_type);
            Assert.AreEqual("timestamp", model.message_timestamp);
            Assert.AreEqual("subscriptionType", model.subscription_type);
            Assert.AreEqual("subscriptionVersion", model.subscription_version);
        }

        [Test]
        public void ConvertMetaDataFromBlankDataTest() 
        {
            var data = "{}";
            metadata model = null;
            model = JsonWrapper.ConvertFromJson<metadata>(data);

            Assert.IsNotNull(model);
            Assert.IsNull(model.message_id);
            Assert.IsNull(model.message_type);
            Assert.IsNull(model.message_timestamp);
            Assert.IsNull(model.subscription_type);
            Assert.IsNull(model.subscription_version);
        }

        [Test]
        public void ConvertRewardTest() 
        {
            var data = "{\n  \"id\": \"12345\",\n  \"title\": \"title\",\n  \"cost\": 123,\n  \"prompt\": \"prompt\"\n}";
            reward model = null;
            model = JsonWrapper.ConvertFromJson<reward>(data);

            Assert.IsNotNull(model);
            Assert.AreEqual("12345", model.id);
            Assert.AreEqual("title", model.title);
            Assert.AreEqual(123, model.cost);
            Assert.AreEqual("prompt", model.prompt);
        }

        [Test]
        public void ConvertRewardFromBlankDataTest() 
        {
            var data = "{}";
            reward model = null;
            model = JsonWrapper.ConvertFromJson<reward>(data);

            Assert.IsNotNull(model);
            Assert.IsNull(model.id);
            Assert.IsNull(model.title);
            Assert.AreEqual(0, model.cost);
            Assert.IsNull(model.prompt);
        }

        [Test]
        public void ConvertSessionTest() 
        {
            var data = "{\n  \"id\": \"12345\",\n  \"status\": \"status\",\n  \"connected_at\": \"20991231\",\n  \"keepalive_timeout_seconds\": 10,\n  \"reconnect_url\": \"http://www.reconnect.com\"\n}";
            session model = null;
            model = JsonWrapper.ConvertFromJson<session>(data);

            Assert.IsNotNull(model);
            Assert.AreEqual("12345", model.id);
            Assert.AreEqual("status", model.status);
            Assert.AreEqual("20991231", model.connected_at);
            Assert.AreEqual(10, model.keepalive_timeout_seconds);
            Assert.AreEqual("http://www.reconnect.com", model.reconnect_url);

        }

        [Test]
        public void ConvertSessionFromBlankDataTest() 
        {
            var data = "{}";
            session model = null;
            model = JsonWrapper.ConvertFromJson<session>(data);

            Assert.IsNotNull(model);
            Assert.IsNull(model.id);
            Assert.IsNull(model.status);
            Assert.IsNull(model.connected_at);
            Assert.AreEqual(0, model.keepalive_timeout_seconds);
            Assert.IsNull(model.reconnect_url);
        }

        [Test]
        public void ConvertSubscriptionTest() 
        {
            var data = "{\n  \"id\": \"12345\",\n  \"status\": \"status\",\n  \"type\": \"type\",\n  \"version\": \"version\",\n  \"cost\": 123,\n  \"created_at\": \"20990101\"\n}";
            subscription model = null;
            model = JsonWrapper.ConvertFromJson<subscription>(data);

            Assert.IsNotNull(model);
            Assert.AreEqual("12345", model.id);
            Assert.AreEqual("status", model.status);
            Assert.AreEqual("type", model.type);
            Assert.AreEqual("version", model.version);
            Assert.AreEqual(123, model.cost);
            Assert.AreEqual("20990101", model.created_at);
        }

        [Test]
        public void ConvertSubscriptionFromBlankDataTest() 
        { 
            var data = "{}";
            subscription model = null;
            model = JsonWrapper.ConvertFromJson<subscription>(data);

            Assert.IsNotNull(model);
            Assert.IsNull(model.id);
            Assert.IsNull(model.status);
            Assert.IsNull(model.type);
            Assert.IsNull(model.version);
            Assert.AreEqual(0, model.cost);
            Assert.IsNull(model.created_at);
        }

        [Test]
        public void ConvertTransportTest() 
        {
            var data = "{\n  \"method\": \"method\",\n  \"session_id\": \"123456\"\n}";
            transport model = null;
            model = JsonWrapper.ConvertFromJson<transport>(data);

            Assert.IsNotNull(model);
            Assert.AreEqual("method", model.method);
            Assert.AreEqual("123456", model.session_id);
        }

        [Test]
        public void ConvertTransportFromBlankDataTest() 
        {
            var data = "{}";
            transport model = null;
            model = JsonWrapper.ConvertFromJson<transport>(data);

            Assert.IsNotNull(model);
            Assert.IsNull(model.method);
            Assert.IsNull(model.session_id);
        }

        [Test]
        public void ConvertPayloadTest() 
        {
            var data = "{\n  \"subscription\": {\n    \"id\": \"12345\",\n    \"status\": \"status\",\n    \"type\": \"type\",\n    \"version\": \"version\",\n    \"cost\": 1234,\n    \"created_at\": \"createdAt\"\n  },\n  \"session\": {\n    \"id\": \"12345\",\n    \"status\": \"status\",\n    \"connected_at\": \"connectedAt\",\n    \"keepalive_timeout_seconds\": 10,\n    \"reconnect_url\": \"url\"\n  },\n  \"event\": {\n    \"user_id\": \"12345\",\n    \"user_login\": \"testUser\",\n    \"user_name\": \"testUserName\",\n    \"broadcaster_user_id\": \"23456\",\n    \"broadcaster_user_login\": \"broadCaster\",\n    \"broadcaster_user_name\": \"broadCasterName\",\n    \"followed_at\": \"1234\",\n    \"\": {},\n    \"reward\": {\n      \"id\": \"23456\",\n      \"title\": \"title\",\n      \"cost\": 123,\n      \"prompt\": \"prompt\"\n    },\n    \"tier\": \"11111\",\n    \"is_gift\": true\n  }\n}";
            payload model = null;
            model = JsonWrapper.ConvertFromJson<payload>(data);

            Assert.IsNotNull(model);

            Assert.AreEqual("12345", model.subscription.id);
            Assert.AreEqual("status", model.subscription.status);
            Assert.AreEqual("type", model.subscription.type);
            Assert.AreEqual("version", model.subscription.version);
            Assert.AreEqual(1234, model.subscription.cost);
            Assert.AreEqual("createdAt", model.subscription.created_at);

            Assert.AreEqual("12345", model.session.id);
            Assert.AreEqual("status", model.session.status);
            Assert.AreEqual("connectedAt", model.session.connected_at);
            Assert.AreEqual(10, model.session.keepalive_timeout_seconds);
            Assert.AreEqual("url", model.session.reconnect_url);

            Assert.AreEqual("12345", model.@event.user_id);
            Assert.AreEqual("testUser", model.@event.user_login);
            Assert.AreEqual("testUserName", model.@event.user_name);
            Assert.AreEqual("23456", model.@event.broadcaster_user_id);
            Assert.AreEqual("broadCaster", model.@event.broadcaster_user_login);
            Assert.AreEqual("broadCasterName", model.@event.broadcaster_user_name);
            Assert.AreEqual("1234", model.@event.followed_at);
            Assert.AreEqual("11111", model.@event.tier);
            Assert.AreEqual(true, model.@event.is_gift);

            Assert.AreEqual("23456", model.@event.reward.id);
            Assert.AreEqual("title", model.@event.reward.title);
            Assert.AreEqual(123, model.@event.reward.cost);
            Assert.AreEqual("prompt", model.@event.reward.prompt);
        }

        [Test]
        public void ConvertPayloadFromBlankDataTest() 
        {
            var data = "{}";
            payload model = null;
            model = JsonWrapper.ConvertFromJson<payload>(data);

            Assert.IsNotNull(model);

            Assert.IsNull(model.subscription);
            Assert.IsNull(model.session);
            Assert.IsNull(model.@event);
        }
    }
}