using Doco.Core.Models;

namespace Doco.Core.Interfaces;

public interface ISearchService
{
    IEnumerable<SearchResult> Search(string query, IDictionary<string, string>? filters = null);
    void IndexDocument(string documentPath);
    void RemoveFromIndex(string documentPath);
}