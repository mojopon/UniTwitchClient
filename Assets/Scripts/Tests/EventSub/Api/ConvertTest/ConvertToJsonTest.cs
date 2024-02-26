using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UniTwitchClient.EventSub;
using UniTwitchClient.EventSub.Api.Models.Raws;
using UnityEngine;

namespace UniTwitchClient.Tests.EventSub.Api
{
    public class ConvertToJsonTest
    {
        [Test]
        public void ConvertRawToJsonTest() 
        {
            var rawModel = new request_subscription_json();
            rawModel.type = "channel.follow";
            rawModel.condition = new condition()
            {
                broadcaster_user_id = "1234",
                moderator_user_id = "2345",
                user_id = "123456",
                from_broadcaster_user_id = "01234",
                to_broadcaster_user_id = "98765",
            };
            rawModel.transport = new transport()
            {
                method = "websocket",
                session_id = "AQoQILE98gtqShGmLD7AM6yJThAB",
            };

            var data = JsonWrapper.ConvertToJson(rawModel);
            var convertedModel = JsonWrapper.ConvertFromJson<request_subscription_json>(data);

            Assert.AreEqual(rawModel.type, convertedModel.type);
            Assert.AreEqual(rawModel.condition.broadcaster_user_id, convertedModel.condition.broadcaster_user_id);
            Assert.AreEqual(rawModel.condition.moderator_user_id, convertedModel.condition.moderator_user_id);
            Assert.AreEqual(rawModel.condition.user_id, convertedModel.condition.user_id);
            Assert.AreEqual(rawModel.condition.from_broadcaster_user_id, convertedModel.condition.from_broadcaster_user_id);
            Assert.AreEqual(rawModel.condition.to_broadcaster_user_id, convertedModel.condition.to_broadcaster_user_id);
            Assert.AreEqual(rawModel.transport.method, convertedModel.transport.method);
            Assert.AreEqual(rawModel.transport.session_id, convertedModel.transport.session_id);
        }
    }
}