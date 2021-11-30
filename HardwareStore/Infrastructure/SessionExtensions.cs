using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace HardwareStore.Infrastructure
{
    public static class SessionExtensions
    {
        public static void SetJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }
        public static T GetJson<T>(this ISession session, string key)
        {
            string data = session.GetString(key);
            return data == null ? default(T) : JsonSerializer.Deserialize<T>(data);
        }
    }
}
