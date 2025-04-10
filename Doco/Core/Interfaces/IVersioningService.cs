using Doco.Core.Models;

namespace Doco.Core.Interfaces
{
    internal interface IVersioningService
    {
        Task<DocumentVersion> CreateVersionAsync(string documentPath);
        Task<DocumentVersion?> GetVersionAsync(string documentPath, string versionId);
        Task<IEnumerable<DocumentVersion>> GetAllVersionsAsync(string documentPath);
        Task RestoreVersionAsync(string documentPath, string versionId);
        Task DeleteVersionAsync(string documentPath, string versionId);
    }
}
