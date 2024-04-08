using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniTwitchClient.EventSub.Api.Models.Raws
{
    [Serializable]
    public class subscription
    {
        public string id;
        public string status;
        public string type;
        public string version;
        public int cost;
        public condition condition;
        public string created_at;
        public transport transport;
    }
}