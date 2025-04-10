using Doco.Core.Interfaces;
using Doco.Core.Models;

namespace Doco.Services.Files.Pdf;

public class GenericDocument : IDocument
{
    public DocumentMetadata Metadata { get; private set; }
    private readonly string _filePath;

    public GenericDocument(DocumentMetadata metadata, string filePath)
    {
        Metadata = metadata;
        _filePath = filePath;
    }

    public Stream GetContentStream() => File.OpenRead(_filePath);
}