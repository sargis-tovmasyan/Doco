namespace Doco.Core.Interfaces
{
    internal interface ISearchService
    {
        Task<IEnumerable<string>> SearchAsync(string query);
        Task IndexDocumentAsync(string documentPath);
        Task RemoveFromIndexAsync(string documentPath);
    }
}
