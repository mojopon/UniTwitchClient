using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace UniTwitchClient.EventSub
{
    public static class JsonWrapper
    {
        public static T ConvertFromJson<T>(string data) where T : class
        {
            return JsonConvert.DeserializeObject<T>(data);
        }

        public static string ConvertToJson(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}
