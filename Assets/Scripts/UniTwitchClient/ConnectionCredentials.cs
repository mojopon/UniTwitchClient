using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionCredentials
{
    public string UserAccessToken { get; }
    public string TwitchUsername { get; }
    public string ClientId { get; }

    public ConnectionCredentials(string userAccessToken, string twitchUsername, string clientId)
    {
        UserAccessToken = userAccessToken;
        TwitchUsername = twitchUsername.ToLower();
        ClientId = clientId;
    }

    public ApiCredentials ToApiCredentials() 
    {
        return new ApiCredentials(UserAccessToken, ClientId);
    }

    public IrcCredentials ToIrcCredentials() 
    {
        return new IrcCredentials(UserAccessToken, TwitchUsername);
    }
}

public class ApiCredentials
{
    public string AuthorizationBearer { get; private set; }
    public string ClientId { get; private set; }
    public ApiCredentials(string userAccessToken, string clientId) 
    {
        AuthorizationBearer = "Bearer " + userAccessToken;
        ClientId = clientId;
    }
}

public class IrcCredentials
{
    public string TwitchOAuthToken { get; }
    public string TwitchUsername { get; }

    public IrcCredentials(string userAccessToken, string twitchUserName)
    {
        if (!userAccessToken.Contains(":"))
        {
            TwitchOAuthToken = $"oauth:{userAccessToken.Replace("oauth", "")}";
        }

        TwitchUsername = twitchUserName;
    }
}
