using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwitchIrcCredentials
{
    public string TwitchOAuthToken { get; }
    public string TwitchUsername { get; }

    public TwitchIrcCredentials(string userAccessToken, string twitchUserName)
    {
        if (!userAccessToken.Contains(":"))
        {
            TwitchOAuthToken = $"oauth:{userAccessToken.Replace("oauth", "")}";
        }

        TwitchUsername = twitchUserName;
    }
}