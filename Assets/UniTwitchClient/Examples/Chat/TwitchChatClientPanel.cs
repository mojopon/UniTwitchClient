using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniTwitchClient.Chat;
using UnityEngine;
using UniRx;
using UnityEngine.UI;

namespace UniTwitchClient.Examples
{
    public class TwitchChatClientPanel : MonoBehaviour
    {
        [SerializeField]
        private TMP_InputField _userTokenField;
        [SerializeField]
        private TMP_InputField _userNameField;
        [SerializeField]
        private TMP_InputField _channelNameField;
        [SerializeField]
        private TMP_InputField _outputField;

        private TwitchChatClient _client;

        public void Connect()
        {
            if (_client != null)
            {
                _client.Dispose();
                _client = null;
            }

            var userToken = _userTokenField.text;
            var username = _userNameField.text;
            var channelName = _channelNameField.text;

            var credentials = new TwitchIrcCredentials(userToken, username);
            _client = new TwitchChatClient(credentials);
            _client.MessageRawAsObservable.Subscribe(x =>
            {
                Debug.Log(x);
            });

            _client.Connect(channelName);
        }

        private void Output(string text)
        {
            _outputField.text += text;
            _outputField.text += Environment.NewLine;

            Debug.Log(text);
        }
    }
}