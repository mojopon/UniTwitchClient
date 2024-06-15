using Newtonsoft.Json;
using System;

namespace UniTwitchClient.EventSub.Api.Models.Raws
{
    [Serializable]
    public class condition
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string broadcaster_user_id;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string user_id;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string moderator_user_id;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string from_broadcaster_user_id;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string to_broadcaster_user_id;
    }
}
