public class TwitchApiCredentials
{
    public string AuthorizationBearer { get; private set; }
    public string ClientId { get; private set; }
    public TwitchApiCredentials(string userAccessToken, string clientId)
    {
        AuthorizationBearer = "Bearer " + userAccessToken;
        ClientId = clientId;
    }
}