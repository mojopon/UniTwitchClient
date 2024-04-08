using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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