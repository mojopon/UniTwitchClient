using Newtonsoft.Json;

namespace UniTwitchClient.EventSub
{
    public static class JsonWrapper
    {
        public static T ConvertFromJson<T>(string data) where T : class
        {
            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new MessageConverter());
            return JsonConvert.DeserializeObject<T>(data, settings);
        }

        public static string ConvertToJson(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}
