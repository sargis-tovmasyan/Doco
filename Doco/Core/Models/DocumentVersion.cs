namespace Doco.Core.Models;

using System;

public class DocumentVersion
{
    public string VersionId { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public string FilePath { get; set; } = string.Empty;
}