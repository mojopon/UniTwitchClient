using System.IO;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniTwitchClient.EventSub;

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

    public void Start()
    {
        string path = Application.dataPath + "/TwitchEventSubClientInputData.txt";
        Debug.Log(path);
        if (File.Exists(path)) 
        {
            string json = File.ReadAllText(path);
            var data = JsonWrapper.ConvertFromJson<TwitchEventSubClientInputData>(json);
            userAccessTokenInputField.text = data.UserAccessToken;
            twitchUserNameInputField.text = data.TwitchUserName;
            clientIdInputField.text = data.ClientId;
            broadcasterUserIdInputField.text = data.BroadcasterUserId;
        }
    }

    public void Connect() 
    {

    }

    public void Disconnect() 
    {

    }

    private class TwitchEventSubClientInputData 
    {
        public string UserAccessToken;
        public string TwitchUserName;
        public string ClientId;
        public string BroadcasterUserId;
    }
}
