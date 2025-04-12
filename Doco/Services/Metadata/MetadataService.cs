using Doco.Core.Interfaces;
using Doco.Core.Models;

namespace Doco.Services.Metadata
{
    public class MetadataService : IMetadataService
    {
        private readonly IMetaDatabase _database;

        public MetadataService(IMetaDatabase database)
        {
            _database = database;
        }

        public DocumentMetadata GetMetadata(string documentPath)
            => _database.GetMetadata(documentPath);

        public void SetMetadata(string documentPath, DocumentMetadata documentMetadata)
            => _database.SetMetadata(documentPath, documentMetadata);

        public void RemoveMetadata(string documentPath)
            => _database.RemoveMetadata(documentPath);
    }
}
