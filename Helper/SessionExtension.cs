using Microsoft.AspNetCore.Http;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace StyleSphere.Helper
{
    public static class SessionExtension
    {
        public static void SetObject<T>(this ISession session, string key, T value)
        {
            try
            {
                var jsonData = JsonSerializer.Serialize(value);
                session.SetString(key, jsonData);
            }
            catch (JsonException ex)
            {
                // Optional: Log to a static logger or rethrow as needed
                Console.WriteLine($"[SessionExtension] Error serializing session object: {ex.Message}");
            }
        }

        public static T? GetObject<T>(this ISession session, string key)
        {
            try
            {
                var value = session.GetString(key);
                return value == null ? default : JsonSerializer.Deserialize<T>(value);
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"[SessionExtension] Error deserializing session object for key '{key}': {ex.Message}");
                return default;
            }
        }
    }
}