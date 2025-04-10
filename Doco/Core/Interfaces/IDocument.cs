namespace Doco.Core.Interfaces;

using Models;
using System.IO;

public interface IDocument
{
    DocumentMetadata Metadata { get; }
    Stream GetContentStream();
}