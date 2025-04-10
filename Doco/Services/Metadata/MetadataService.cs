using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using Doco.Core.Interfaces;

namespace Doco.Services
{
    public class MetadataService : IMetadataService
    {
        private const string MetadataFileExtension = ".metadata.json";

        private static readonly JsonSerializerOptions SerializerOptions = new JsonSerializerOptions
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

        public async Task<IDictionary<string, string>> GetMetadataAsync(string documentPath)
        {
            string metadataFilePath = GetMetadataFilePath(documentPath);

            if (!File.Exists(metadataFilePath))
            {
                return new Dictionary<string, string>();
            }

            string json = await File.ReadAllTextAsync(metadataFilePath);
            return JsonSerializer.Deserialize<Dictionary<string, string>>(json, SerializerOptions)
                   ?? new Dictionary<string, string>();
        }

        public async Task SetMetadataAsync(string documentPath, IDictionary<string, string> metadata)
        {
            string metadataFilePath = GetMetadataFilePath(documentPath);

            var existingMetadata = await GetMetadataAsync(documentPath);
            foreach (var kvp in metadata)
            {
                existingMetadata[kvp.Key] = kvp.Value;
            }

            string json = JsonSerializer.Serialize(existingMetadata, SerializerOptions);
            await File.WriteAllTextAsync(metadataFilePath, json);
        }

        public async Task RemoveMetadataAsync(string documentPath, IEnumerable<string> keys)
        {
            string metadataFilePath = GetMetadataFilePath(documentPath);

            if (!File.Exists(metadataFilePath))
            {
                return;
            }

            var existingMetadata = await GetMetadataAsync(documentPath);
            foreach (var key in keys)
            {
                existingMetadata.Remove(key);
            }

            string json = JsonSerializer.Serialize(existingMetadata, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(metadataFilePath, json);
        }

        private string GetMetadataFilePath(string documentPath)
        {
            return Path.ChangeExtension(documentPath, MetadataFileExtension);
        }
    }
}
