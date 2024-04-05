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
        Debug.Log("Get Subs");
    }

    public void DeleteAllSubs() 
    {
        Debug.Log("Delete All Subs");
    }

    TwitchEventSubApiClient CreateClient() 
    {
        var credentials = new ConnectionCredentials(userAccessTokenInputField.text, "user", clientIdInputField.text);
        return new TwitchEventSubApiClient(credentials.ToApiCredentials());
    }
}
