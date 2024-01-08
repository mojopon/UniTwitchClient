using Codice.Client.Common.GameUI;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

namespace UniTwitchClient.Tests
{
    public class CredentialTest
    {
        private string userName = "anomaloris";
        private string userAccessToken = "1wteiz6quzbjfpr9sbv7pdqpja7jqr";
        private string clientId = "qmsa7usjzdisgavghu1tirlzhzw98n";

        private ConnectionCredentials _connectionCredentials;

        [SetUp]
        public void SetUp()
        {
            _connectionCredentials = new ConnectionCredentials(userAccessToken, userName, clientId);
        }

        [Test]
        public void ConnectionCredentialsToApiCredentialsTest()
        {
            var apiCredentials = _connectionCredentials.ToApiCredentials();

            Assert.AreEqual("Bearer 1wteiz6quzbjfpr9sbv7pdqpja7jqr", apiCredentials.AuthorizationBearer);
            Assert.AreEqual("qmsa7usjzdisgavghu1tirlzhzw98n", apiCredentials.ClientId);
        }

        [Test]
        public void ConnectionCredentialsToIrcCredentialsTest()
        {
            var ircCredentials = _connectionCredentials.ToIrcCredentials();

            Assert.AreEqual("oauth:1wteiz6quzbjfpr9sbv7pdqpja7jqr", ircCredentials.TwitchOAuthToken);
            Assert.AreEqual("anomaloris", ircCredentials.TwitchUsername);
        }
    }
}
