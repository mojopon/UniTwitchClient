using UnityEngine;
using UniRx;
using UniTwitchClient.EventSub;
using UniTwitchClient.EventSub.Api;
using UniTwitchClient.EventSub.WebSocket;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;

public class Example : MonoBehaviour
{
    public string twitchUserName;
    public string userAccessToken;
    public string clientId;
    public string broadcasterUserId;

    private TwitchEventSubClient _twitchEventSubClient;

    public void Connect()
    {
        ConnectAsync();
    }

    public async void ConnectAsync() 
    {
        if(_twitchEventSubClient != null) { return; }

        var connectionCredential = new TwitchApiCredentials(userAccessToken, clientId);
        var wsClient = new TwitchEventSubWebsocketClient();
        wsClient.DebugMode = true;
        var apiClient = new TwitchEventSubApiClient(connectionCredential);
        apiClient.DebugMode = true;
        _twitchEventSubClient = new TwitchEventSubClient(wsClient, apiClient);

        _twitchEventSubClient.OnChannelFollowAsObservable.Subscribe(x =>
        {
            Debug.Log("the channel is followed by " + x.UserName + " !, UserId: " + x.UserId);
        });

        _twitchEventSubClient.OnChannelSubscribeAsObservable.Subscribe(x =>
        {
            Debug.Log("the channel is subscribed by " + x.UserName + " !, UserId: " + x.UserId);
        });
        _twitchEventSubClient.OnChannelPointsCustomRewardRedemptionAddAsObservable.Subscribe(x =>
        {
            Debug.Log($"the channel points custom reward redemption is added!\nTitle:{x.RewardTitle}\nPrompt:{x.RewardPrompt}\nCost:{x.RewardCost}");
        });

        await _twitchEventSubClient.ConnectChannelAsync(broadcasterUserId);

        Debug.Log("Connected.");
    }

    public void Disconnect()
    {
        DisconnectAsync();
    }

    private bool _disconnecting = false;
    private async void DisconnectAsync() 
    {
        if(_disconnecting || _twitchEventSubClient == null) return;

        _disconnecting = true;
        await _twitchEventSubClient.DisconnectChannelAsync();

        _twitchEventSubClient.Dispose();
        _twitchEventSubClient = null;
        _disconnecting = false;

        Debug.Log("Disconnected.");
    }
}
