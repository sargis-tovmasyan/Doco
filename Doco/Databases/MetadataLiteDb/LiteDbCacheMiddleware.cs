using System.Collections.Concurrent;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace Doco.Databases.MetadataLiteDb
{
    public class LiteDbCacheMiddleware
    {
        private readonly ConcurrentDictionary<string, CacheEntry> _cache = new();

        //Solving AOT issues with JsonSerializerOptions as by default it's using Reflection
        private readonly JsonSerializerOptions _serializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            TypeInfoResolver = new DefaultJsonTypeInfoResolver
            {
                Modifiers =
                {
                    typeInfo =>
                    {
                        if (typeInfo.Type == typeof(Dictionary<string, string>) && typeInfo.Kind == JsonTypeInfoKind.Object)
                        {
                            typeInfo.Properties.Clear(); // Customize properties if needed
                        }
                    }
                }
            }
        };

        string GetCacheKey(string query, object parameters) =>
            $"{query}:{JsonSerializer.Serialize(parameters, _serializerOptions)}";

        public object HandleRequest(string query, object parameters, Func<object> dbCall, Func<DateTime> getLastModified)
        {
            //TODO: Optimize
            var key = GetCacheKey(query, parameters);
            var currentDbLastModified = getLastModified();

            if (_cache.TryGetValue(key, out var entry))
            {
                if (entry.LastModified >= currentDbLastModified)
                {
                    return entry.Result; // Cached and still fresh
                }
            }

            var result = dbCall();
            _cache[key] = new CacheEntry
            {
                Result = result,
                LastModified = currentDbLastModified
            };
            return result;
        }
    }

}
