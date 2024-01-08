using UnityEngine;
using UniRx;
using UniTwitchClient.EventSub;

public class Example : MonoBehaviour
{
    public string twitchUserName;
    public string userAccessToken;
    public string clientId;
    public string broadcasterUserId;

    private TwitchEventSubClient _twitchEventSubClient;
    void Start()
    {
        var connectionCredential = new ConnectionCredentials(userAccessToken, twitchUserName, clientId);
        _twitchEventSubClient = new TwitchEventSubClient(connectionCredential);
        _twitchEventSubClient.DebugMode = true;

        _twitchEventSubClient.SubscribeChannelFollow(broadcasterUserId);
        _twitchEventSubClient.SubscribeChannelSubscribe(broadcasterUserId);

        _twitchEventSubClient.OnChannelFollowAsObservable.Subscribe(x => 
        {
            Debug.Log("the channel is followed by " + x.UserName + " !, UserId: " + x.UserId);
        });

        _twitchEventSubClient.OnChannelSubscribeAsObservable.Subscribe(x =>
        {
            Debug.Log("the channel is subscribed by " + x.UserName + " !, UserId: " + x.UserId);
        });

        _twitchEventSubClient.Connect();
    }

    private void OnDestroy()
    {
        _twitchEventSubClient.Dispose();
    }
}
