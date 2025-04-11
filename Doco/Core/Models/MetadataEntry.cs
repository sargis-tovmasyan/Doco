public class MetadataEntry
{
    public int Id { get; set; }
    public string DocumentPath { get; set; }
    public IDictionary<string, string> Metadata { get; set; } = new Dictionary<string, string>();
}