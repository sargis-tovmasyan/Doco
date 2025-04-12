using Doco.Core;
using Doco.Core.Exceptions;
using Doco.Core.Interfaces;
using Doco.Core.Models;

namespace Doco.Services.Files.Pdf;

public class PdfDocumentService : IDocumentService
{
    private readonly IMetadataService _metadataService;

    public PdfDocumentService(IMetadataService metadataService)
    {
        _metadataService = metadataService;
    }

    public IDocument LoadDocument(string filePath)
    {
        if (!File.Exists(filePath)) throw new FileNotFoundException(filePath);
        if (!filePath.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
            throw new UnsupportedDocumentTypeException("Only PDF is supported.");

        var metadata = new DocumentMetadata
        {
            FileName = Path.GetFileName(filePath),
            Type = DocumentType.Pdf,
            Size = new FileInfo(filePath).Length,
            CreatedAt = File.GetCreationTime(filePath)
        };

        return new GenericDocument(metadata, filePath);
    }

    public void SaveDocument(IDocument document, string destinationPath)
    {
        using var input = document.GetContentStream();
        using var output = File.Create(destinationPath);
        input.CopyTo(output);

        _metadataService.SetMetadata(destinationPath, document.Metadata);
    }

    public void DeleteDocument(string filePath)
    {
        if (File.Exists(filePath)) File.Delete(filePath);
    }
}