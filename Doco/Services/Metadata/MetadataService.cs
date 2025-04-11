using Doco.Core.Interfaces;

namespace Doco.Services.Metadata
{
    public class MetadataService : IMetadataService
    {
        private readonly IMetaDatabase _database;

        public MetadataService(IMetaDatabase database)
        {
            _database = database;
        }

        public IDictionary<string, string> GetMetadataAsync(string documentPath)
            => _database.GetMetadata(documentPath);

        public void SetMetadataAsync(string documentPath, IDictionary<string, string> metadata)
            => _database.SetMetadata(documentPath, metadata);

        public void RemoveMetadataAsync(string documentPath, IEnumerable<string> keys)
            => _database.RemoveMetadata(documentPath, keys);
    }
}
