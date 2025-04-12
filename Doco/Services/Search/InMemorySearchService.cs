using Doco.Core.Interfaces;
using Doco.Core.Models;

namespace Doco.Services.Search;

public class InMemorySearchService : ISearchService
{
    private readonly Dictionary<string, string> _index = new();
    private readonly IMetadataService _metadataService;

    public InMemorySearchService(IMetadataService metadataService)
    {
        _metadataService = metadataService;
    }

    public void IndexDocument(string documentPath)
    {
        if (!File.Exists(documentPath))
            return;

        var content = File.ReadAllText(documentPath);
        _index[documentPath] = content;
    }

    public void RemoveFromIndex(string documentPath)
    {
        _index.Remove(documentPath);
    }

    public IEnumerable<SearchResult> Search(string query, IDictionary<string, string>? filters = null)
    {
        var results = new List<SearchResult>();

        foreach (var entry in _index)
        {
            if (!entry.Value.Contains(query, StringComparison.OrdinalIgnoreCase))
                continue;

            var metadata = _metadataService.GetMetadata(entry.Key);

            if (filters != null && filters.Any(f => !metadata.Tags.TryGetValue(f.Key, out var val) || val != f.Value))
                continue;

            // Bonus score if query appears in tags
            var queryLower = query.ToLowerInvariant();

            var tagHits = metadata.Tags.Count(kvp =>
                kvp.Key.Contains(queryLower) || kvp.Value.Contains(queryLower));

            results.Add(new SearchResult
            {
                DocumentPath = entry.Key,
                Score = tagHits,
                Metadata = metadata
            });
        }

        return results;
    }
}