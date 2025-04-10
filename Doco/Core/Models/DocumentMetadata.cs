namespace Doco.Core.Models;

using System;

public class DocumentMetadata
{
    public string FileName { get; set; } = string.Empty;
    public DocumentType Type { get; set; } = DocumentType.Unknown;
    public string ContentType { get; set; } = "application/octet-stream";
    public long Size { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastModifiedAt { get; set; }
}