using UniTwitchClient.EventSub.WebSocket;
using UniTwitchClient.EventSub.WebSocket.Models.Raws;

public static class RawToModelConverter
{
    public static Welcome ConvertRawToModel(this welcome_raw source)
    {
        Welcome welcomeMessage = new Welcome();
        welcomeMessage.MessageType = source.metadata.message_type;
        welcomeMessage.SessionId = source.payload.session.id;
        welcomeMessage.KeepAliveTimeoutSeconds = source.payload.session.keepalive_timeout_seconds;
        return welcomeMessage;
    }

    public static WebSocketMessageBase ConvertRawToModel(this message_base source)
    {
        WebSocketMessageBase messageBase = new WebSocketMessageBase();
        messageBase.MessageType = source.metadata.message_type;
        return messageBase;
    }

    public static KeepAlive ConvertRawToModel(this keepalive_raw source)
    {
        KeepAlive keepaliveMessage = new KeepAlive();
        keepaliveMessage.MessageType = source.metadata.message_type;
        return keepaliveMessage;
    }

    public static Notification ConvertRawToModel(this notification_raw source)
    {
        var eventSource = source.payload.@event;
        Notification notification = new Notification();
        notification.MessageType = source.metadata.message_type;
        notification.SubscriptionType = SubscriptionTypeConverter.ToSubscriptionType(source.payload.subscription.type);
        notification.UserId = eventSource.user_id;
        notification.UserName = eventSource.user_name;
        notification.UserLogin = eventSource.user_login;
        notification.BroadCasterUserId = eventSource.broadcaster_user_id;
        notification.BroadCasterUserName = eventSource.broadcaster_user_name;
        notification.BroadCasterUserLogin = eventSource.broadcaster_user_login;
        notification.RewardId = eventSource.reward.id;
        notification.RewardTitle = eventSource.reward.title;
        notification.RewardPrompt = eventSource.reward.prompt;
        notification.RewardCost = eventSource.reward.cost;
        notification.IsGift = eventSource.is_gift;
        notification.Tier = eventSource.tier;

        return notification;
    }
}
