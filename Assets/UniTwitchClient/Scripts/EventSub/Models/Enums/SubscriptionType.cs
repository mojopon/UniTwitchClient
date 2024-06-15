public enum SubscriptionType
{
    None,
    ChannelUpdate,
    ChannelFollow,
    ChannelCheer,
    ChannelSubscribe,
    ChannelSubscriptionGift,
    ChannelSubscriptionMessage,
    ChannelPointsCustomRewardRedemptionAdd,
}

public static class SubscriptionTypeConverter
{
    public static SubscriptionType ToSubscriptionType(string name)
    {
        switch (name)
        {
            case "channel.update":
                return SubscriptionType.ChannelUpdate;
            case "channel.follow":
                return SubscriptionType.ChannelFollow;
            case "channel.cheer":
                return SubscriptionType.ChannelCheer;
            case "channel.subscribe":
                return SubscriptionType.ChannelSubscribe;
            case "channel.subscription.gift":
                return SubscriptionType.ChannelSubscriptionGift;
            case "channel.subscription.message":
                return SubscriptionType.ChannelSubscriptionMessage;
            case "channel.channel_points_custom_reward_redemption.add":
                return SubscriptionType.ChannelPointsCustomRewardRedemptionAdd;
            default:
                return SubscriptionType.None;
        }
    }

    public static string ToName(this SubscriptionType type)
    {
        switch (type)
        {
            case SubscriptionType.ChannelUpdate:
                return "channel.update";
            case SubscriptionType.ChannelFollow:
                return "channel.follow";
            case SubscriptionType.ChannelCheer:
                return "channel.cheer";
            case SubscriptionType.ChannelSubscribe:
                return "channel.subscribe";
            case SubscriptionType.ChannelSubscriptionGift:
                return "channel.subscription.gift";
            case SubscriptionType.ChannelSubscriptionMessage:
                return "channel.subscription.message";
            case SubscriptionType.ChannelPointsCustomRewardRedemptionAdd:
                return "channel.channel_points_custom_reward_redemption.add";
            default:
                return "";
        }
    }
}
