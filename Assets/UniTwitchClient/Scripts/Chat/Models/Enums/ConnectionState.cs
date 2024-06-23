namespace UniTwitchClient.Chat.Models
{
    public enum ConnectionState
    {
        Idle = 0,
        Connecting,
        Connected,
        Disconnected,
        Error,
    }
}
