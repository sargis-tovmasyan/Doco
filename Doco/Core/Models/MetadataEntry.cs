using LiteDB;

namespace Doco.Core.Models;

public class MetadataEntry
{
    [BsonId]
    public ObjectId Id { get; set; }
    public required string DocumentPath { get; set; }
    public required DateTime LastModified { get; set; }
    public required DocumentMetadata Metadata { get; set; } = new();
}