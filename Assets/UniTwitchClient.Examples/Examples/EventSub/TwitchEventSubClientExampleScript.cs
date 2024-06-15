using System.IO;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniTwitchClient.EventSub;
using UnityEngine.UI;
using UnityEditor.PackageManager;
using UniTwitchClient.EventSub.WebSocket;
using UniTwitchClient.EventSub.Api;
using UniRx;
using System;

namespace UniTwitchClient.Examples
{
    public class TwitchEventSubClientExampleScript : MonoBehaviour
    {
        [SerializeField]
        private TMP_InputField userAccessTokenInputField;
        [SerializeField]
        private TMP_InputField clientIdInputField;
        [SerializeField]
        private TMP_InputField broadcasterUserIdInputField;
        [SerializeField]
        private Toggle connectToLocalServerToggle;
        [SerializeField]
        private TMP_InputField outputField;

        private TwitchEventSubClient _client;
        private CompositeDisposable _disposables;

        public void Start()
        {
            string path = Application.dataPath + "/UniTwitchClientExampleSceneInputData.txt";

            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                var data = JsonWrapper.ConvertFromJson<UniTwitchClientExampleSceneInputData>(json);
                userAccessTokenInputField.text = data.UserAccessToken;
                clientIdInputField.text = data.ClientId;
                broadcasterUserIdInputField.text = data.BroadcasterUserId;
                connectToLocalServerToggle.isOn = data.ConnectToLocalServer;
            }
        }

        public void Connect()
        {
            if (_client != null)
            {
                return;
            }

            _client = CreateTwitchEventSubClient();
            _client.OutputLogOnUnity = true;
            SubscribeClient();
            ConnectAsync();
        }

        private void SubscribeClient()
        {
            _disposables = new CompositeDisposable();

            _client.OnChannelFollowAsObservable.Subscribe(x =>
            {
                outputField.text += $"an user followed. User Name:{x.UserName}";
                outputField.text += Environment.NewLine;
            }).AddTo(_disposables);

            _client.OnChannelPointsCustomRewardRedemptionAddAsObservable.Subscribe(x =>
            {
                outputField.text += $"an user redeemed a reward. User Name:{x.UserName}, Reward Title:{x.RewardTitle}";
                outputField.text += Environment.NewLine;
            }).AddTo(_disposables);

            _client.OnChannelSubscribeAsObservable.Subscribe(x =>
            {
                outputField.text += $"an user subscribed the channel. User Name:{x.UserName}";
                outputField.text += Environment.NewLine;
            }).AddTo(_disposables);
        }

        private async void ConnectAsync()
        {
            await _client.ConnectChannelAsync(broadcasterUserIdInputField.text);
        }

        public void Disconnect()
        {
            DisconnectAsync();
            _disposables.Dispose();
        }

        private async void DisconnectAsync()
        {
            try
            {
                await _client.DisconnectChannelAsync();
            }
            catch
            {
                Debug.LogError("Something wrong is happened while disconnecting.");
            }
            finally
            {
                _client.Dispose();
                _client = null;
            }
        }

        private TwitchEventSubClient CreateTwitchEventSubClient()
        {
            var credentials = CreateTwitchApiCredentials();

            if (connectToLocalServerToggle.isOn)
            {
                var wsClient = new TwitchEventSubWebsocketClient();
                var apiClient = new TwitchEventSubApiClient(credentials);
                wsClient.ConnectToLocalCLIServer = true;
                apiClient.ConnectToLocalCLIServer = true;
                return new TwitchEventSubClient(wsClient, apiClient);
            }
            else
            {
                return new TwitchEventSubClient(credentials);
            }
        }

        private TwitchApiCredentials CreateTwitchApiCredentials()
        {
            return new TwitchApiCredentials(userAccessTokenInputField.text, clientIdInputField.text);
        }
    }
}