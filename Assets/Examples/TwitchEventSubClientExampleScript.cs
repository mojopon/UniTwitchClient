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

public class TwitchEventSubClientExampleScript : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField userAccessTokenInputField;
    [SerializeField]
    private TMP_InputField twitchUserNameInputField;
    [SerializeField]
    private TMP_InputField clientIdInputField;
    [SerializeField]
    private TMP_InputField broadcasterUserIdInputField;
    [SerializeField]
    private Toggle connectToLocalServerToggle;

    private TwitchEventSubClient _client;

    public void Start()
    {
        string path = Application.dataPath + "/TwitchEventSubClientInputData.txt";

        if (File.Exists(path)) 
        {
            string json = File.ReadAllText(path);
            var data = JsonWrapper.ConvertFromJson<TwitchEventSubClientInputData>(json);
            userAccessTokenInputField.text = data.UserAccessToken;
            twitchUserNameInputField.text = data.TwitchUserName;
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
        ConnectAsync();
    }

    private async void ConnectAsync() 
    {
        await _client.ConnectChannelAsync(broadcasterUserIdInputField.text);
    }

    public void Disconnect() 
    {
        DisconnectAsync();
    }

    private async void DisconnectAsync() 
    {
        try
        {
            await _client.DisconnectChannel();
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
        var credentials = CreateConnectionCredentials();

        if (connectToLocalServerToggle.isOn)
        {
            var wsClient = new TwitchEventSubWebsocketClient();
            var apiClient = new TwitchEventSubApiClient(credentials.ToApiCredentials());
            wsClient.DebugMode = true;
            apiClient.DebugMode = true;
            return new TwitchEventSubClient(wsClient, apiClient);
        }
        else 
        {
            return new TwitchEventSubClient(credentials);
        }
    }

    private ConnectionCredentials CreateConnectionCredentials() 
    {
        return new ConnectionCredentials(userAccessTokenInputField.text, twitchUserNameInputField.text, clientIdInputField.text);
    }

    private class TwitchEventSubClientInputData 
    {
        public string UserAccessToken;
        public string TwitchUserName;
        public string ClientId;
        public string BroadcasterUserId;
        public bool ConnectToLocalServer;
    }
}
