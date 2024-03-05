using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UniTwitchClient.EventSub;
using UniTwitchClient.EventSub.Api.Mocks;
using UniTwitchClient.EventSub.Mocks;
using UnityEngine;

namespace UniTwitchClient.Tests.EventSub
{
    public class TwitchEventSubClientTest
    {
        [Test]
        public void ReceiveWelcomeMessageTest() 
        {
            var wsClient = new TwitchEventSubWebSocketClientMock();
            var apiClient = new TwitchEventSubApiClientMock();
            var client = new TwitchEventSubClient(wsClient, apiClient);

        }
    }
}