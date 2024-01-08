using NUnit.Framework;
using UniTwitchClient.EventSub;
using UniTwitchClient.EventSub.WebSocket.Models.Raws;

namespace UniTwitchClient.Tests.EventSub.WebSocket
{
    public class ConvertKeepAliveTest
    {
        [Test]
        public void ConvertToKeepAliveTest()
        {
            string data = "{\"metadata\":{\"message_id\":\"6fe2fbb5-ff4b-6498-c810-1fb7b4283775\",\"message_type\":\"session_keepalive\",\"message_timestamp\":\"2023-12-06T04:38:28.72119Z\"},\"payload\":{}}";
            var rawModel = JsonConverter.ConvertFromJson<keepalive_raw>(data);
            var model = rawModel.ConvertRawToModel();

            Assert.AreEqual("session_keepalive", model.MessageType);
        }
    }
}