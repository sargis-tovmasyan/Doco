using Doco.Core.Interfaces;
using LiteDB;

namespace Doco.Services.Metadata
{
    public class MetaDatabase : IMetaDatabase ,IDisposable
    {
        private readonly LiteDatabase _database;

        private const string COLLECTION_NAME = "metadata";

        public MetaDatabase(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString)) throw new ArgumentException("Connection string cannot be null or empty.", nameof(connectionString));

            _database = new LiteDatabase(connectionString);
        }

        public IDictionary<string, string> GetMetadata(string documentPath)
        {
            var collection = _database.GetCollection<MetadataEntry>(COLLECTION_NAME);
            var entry = collection.FindOne(e => e.DocumentPath == documentPath);
            return entry?.Metadata ?? new Dictionary<string, string>();
        }

        public void SetMetadata(string documentPath, IDictionary<string, string> metadata)
        {
            var collection = _database.GetCollection<MetadataEntry>(COLLECTION_NAME);
            var entry = collection.FindOne(e => e.DocumentPath == documentPath);

            if (entry == null)
            {
                entry = new MetadataEntry
                {
                    DocumentPath = documentPath,
                    Metadata = metadata
                };
                collection.Insert(entry);
            }
            else
            {
                entry.Metadata = metadata;
                collection.Update(entry);
            }
        }

        public void RemoveMetadata(string documentPath, IEnumerable<string> keys)
        {
            var collection = _database.GetCollection<MetadataEntry>(COLLECTION_NAME);
            var entry = collection.FindOne(e => e.DocumentPath == documentPath);

            if (entry != null)
            {
                foreach (var key in keys)
                {
                    entry.Metadata.Remove(key);
                }
                collection.Update(entry);
            }
        }

        public void Dispose()
        {
            _database.Dispose();
        }
    }
}
