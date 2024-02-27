using NUnit.Framework;
using UniTwitchClient.EventSub;
using UniTwitchClient.EventSub.WebSocket.Models.Raws;

namespace UniTwitchClient.Tests.EventSub.WebSocket
{
    public class ConvertWelcomeTest
    {
        [Test]
        public void ConvertWelcomeFromBlankDataTest() 
        {
            string data = "{}";
            var rawModel = JsonWrapper.ConvertFromJson<welcome_raw>(data);
            var model = rawModel.ConvertRawToModel();

            Assert.IsNotNull(model);
        }

        [Test]
        public void ConvertToWelcomeTest()
        {
            string data = "{\"metadata\":{\"message_id\":\"cee70190-9024-df2f-3623-83f5ac0813aa\",\"message_type\":\"session_welcome\",\"message_timestamp\":\"2023-12-06T04:13:19.1999374Z\"},\"payload\":{\"session\":{\"id\":\"a07f635d_7b043750\",\"status\":\"connected\",\"keepalive_timeout_seconds\":10,\"reconnect_url\":null,\"connected_at\":\"2023-12-06T04:13:19.1989356Z\"}}}";
            var rawModel = JsonWrapper.ConvertFromJson<welcome_raw>(data);
            var model = rawModel.ConvertRawToModel();

            Assert.AreEqual("session_welcome", model.MessageType);
            Assert.AreEqual("a07f635d_7b043750", model.SessionId);
            Assert.AreEqual(10, model.KeepAliveTimeoutSeconds);
        }
    }
}