using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using System.Text.Json.Serialization;

namespace Guestline.Booking.App.Helpers
{
    internal static class JsonHelper
    {
        internal static List<T> LoadCollectionFromFile<T>(string path)
        {
            return LoadCollectionFromString<T>(File.ReadAllText(path));
        }

        internal static List<T> LoadCollectionFromString<T>(string jsonContent)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                TypeInfoResolver = new DefaultJsonTypeInfoResolver
                {
                    Modifiers = { ReadDatesModifier }
                }
            };
            return JsonSerializer.Deserialize<List<T>>(jsonContent, options) ?? new List<T>();
        }

        static void ReadDatesModifier(JsonTypeInfo jsonTypeInfo)
        {
            if (jsonTypeInfo.Type == typeof(uint))
            {
                jsonTypeInfo.NumberHandling = JsonNumberHandling.AllowReadingFromString;
            }
        }
    }
}
