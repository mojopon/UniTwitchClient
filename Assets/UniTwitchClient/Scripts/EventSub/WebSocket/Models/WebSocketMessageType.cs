namespace UniTwitchClient.EventSub.WebSocket
{
    public enum WebSocketMessageType
    {
        None = 0,
        SessionWelcome,
        SessionKeepAlive,
        Ping,
        Notification,
        Reconnect,
        Revocation,
        Close,
    }
}