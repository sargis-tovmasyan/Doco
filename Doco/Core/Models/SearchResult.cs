namespace Doco.Core.Models
{
    public class SearchResult
    {
        public string DocumentPath { get; set; } = string.Empty;
        public int Score { get; set; }
        public DocumentMetadata Metadata { get; set; } = new DocumentMetadata();
    }

}
