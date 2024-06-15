using System;

namespace UniTwitchClient.EventSub.Api.Models.Raws
{
    [Serializable]
    public class request_subscription_json
    {
        public string type;
        public string version;
        public condition condition;
        public transport transport;
    }
}