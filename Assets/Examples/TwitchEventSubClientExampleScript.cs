using System.IO;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
            Debug.Log("the file is exists");
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

    }
}
