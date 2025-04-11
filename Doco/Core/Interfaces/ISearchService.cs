using Doco.Core.Models;

namespace Doco.Core.Interfaces
{
    internal interface ISearchService
    {
        Task<IEnumerable<SearchResult>> SearchAsync(string query, IDictionary<string, string>? filters = null);
        Task IndexDocumentAsync(string documentPath);
        Task RemoveFromIndexAsync(string documentPath);
    }
}
