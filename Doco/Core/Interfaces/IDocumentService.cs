namespace Doco.Core.Interfaces;

internal interface IDocumentService
{
    IDocument LoadDocument(string filePath);
    void SaveDocument(IDocument document, string destinationPath);
    void DeleteDocument(string filePath);
}