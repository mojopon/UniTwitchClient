using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UniTwitchClient.EventSub;
using UniTwitchClient.EventSub.Api;
using UnityEngine;
using UnityEngine.UI;

public class EventSubSubscriptionManagementSceneScript : MonoBehaviour
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

    TwitchEventSubApiClient _client;

    // Start is called before the first frame update
    void Start()
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

    public void GetSubs() 
    {
        GetSubsAsync();
    }

    private async void GetSubsAsync() 
    {
        var result = await CreateClient().GetEventSubSubscriptionsAsync();

        outputField.text = "";
        foreach (var subscription in result.Subscriptions) 
        {
            outputField.text += $"Id:{subscription.Id}, + Session Id:{subscription.SessionId}, Type:{subscription.SubscriptionType}, Status:{subscription.Status}";
            outputField.text += Environment.NewLine;
        }
    }

    public void DeleteAllSubs() 
    {
        DeleteAllSubsAsync();
    }

    private async void DeleteAllSubsAsync() 
    {
        var client = CreateClient();
        var result = await client.GetEventSubSubscriptionsAsync();
        await client.DeleteEventSubSubscriptionsAsync(result);

        Debug.Log("EventSub Subscriptions Deleted.");
    }

    TwitchEventSubApiClient CreateClient() 
    {
        var credentials = new TwitchApiCredentials(userAccessTokenInputField.text, clientIdInputField.text);
        var client = new TwitchEventSubApiClient(credentials);
        client.ConnectToLocalCLIServer = connectToLocalServerToggle.isOn;
        return client;
    }
}
