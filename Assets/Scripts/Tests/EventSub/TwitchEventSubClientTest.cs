using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniTwitchClient.Tests.EventSub
{
    public class TwitchEventSubClientTest
    {
        [Test]
        public void ReceiveWelcomeMessageTest() 
        {
            var wsClient = new TwitchEventSubWebSocketClientMock();
        }
    }
}