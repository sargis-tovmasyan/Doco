using Doco.Core.Interfaces;
using Doco.Core.Models;
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

        public DocumentMetadata GetMetadata(string documentPath)
        {
            var collection = _database.GetCollection<MetadataEntry>(COLLECTION_NAME);
            var entry = collection.FindOne(e => e.DocumentPath == documentPath);
            return entry?.Metadata ?? new DocumentMetadata();
        }

        public void SetMetadata(string documentPath, DocumentMetadata documentMetadata)
        {
            var collection = _database.GetCollection<MetadataEntry>(COLLECTION_NAME);
            var entry = collection.FindOne(e => e.DocumentPath == documentPath);

            if (entry == null)
            {
                entry = new MetadataEntry
                {
                    DocumentPath = documentPath,
                    Metadata = documentMetadata,
                    LastModified = DateTime.Now
                };

                collection.Insert(entry);
            }
            else
            {
                if (entry.Metadata.LastModifiedAt == documentMetadata.LastModifiedAt)
                    return; // Preventing unnecessary updates

                entry.Metadata = documentMetadata;
                entry.LastModified = DateTime.Now;

                collection.Update(entry);
            }
        }

        public bool RemoveMetadata(string documentPath)
        {
            bool result = false;

            var collection = _database.GetCollection<MetadataEntry>(COLLECTION_NAME);
            var entry = collection.FindOne(e => e.DocumentPath == documentPath);

            if (entry != null)
            {
                result = collection.Delete(new BsonValue(entry.Id));
            }

            return result;  
        }

        public void Dispose()
        {
            _database.Dispose();
        }
    }
}
