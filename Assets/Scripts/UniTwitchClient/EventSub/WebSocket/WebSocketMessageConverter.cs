using UniTwitchClient.EventSub;
using UniTwitchClient.EventSub.WebSocket;
using UniTwitchClient.EventSub.WebSocket.Models.Raws;

/// <summary>
/// Convert json data to models that has been used in UniTwitch.
/// </summary>
public static class WebSocketMessageConverter 
{
    public static Welcome ConvertToWelcomeMessage(string data) 
    {
        return JsonWrapper.ConvertFromJson<welcome_raw>(data).ConvertRawToModel();
    }

    public static KeepAlive ConvertToKeepAliveMessage(string data) 
    {
        return JsonWrapper.ConvertFromJson<keepalive_raw>(data).ConvertRawToModel();
    }

    public static Notification ConvertToNotification(string data) 
    {
        return JsonWrapper.ConvertFromJson<notification_raw>(data).ConvertRawToModel();
    }

    public static WebSocketMessageBase ConvertToMessageBase(string data) 
    {
        return JsonWrapper.ConvertFromJson<message_base>(data).ConvertRawToModel();
    }

    public static WebSocketMessageType GetMessageType(string data)
    {
        var messageBase = ConvertToMessageBase(data);
        if (messageBase == null) return WebSocketMessageType.None;

        switch (messageBase.MessageType) 
        {
            case "session_welcome": 
                {
                    return WebSocketMessageType.SessionWelcome;
                }
            case "session_keepalive":
                {
                    return WebSocketMessageType.SessionKeepAlive;
                }
            case "notification":
                {
                    return WebSocketMessageType.Notification;
                }
            case "session_reconnect":
                {
                    return WebSocketMessageType.Reconnect;
                }
            case "revocation":
                {
                    return WebSocketMessageType.Revocation;
                }
            default:
                {
                    return WebSocketMessageType.None;
                }
        }
    }
}
