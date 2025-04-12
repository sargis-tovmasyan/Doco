using Doco.Core.Models;
namespace Doco.Core.Interfaces;
using System.Collections.Generic;

public interface IMetadataService
{
    DocumentMetadata GetMetadata(string documentPath);
    void SetMetadata(string documentPath, DocumentMetadata documentMetadata);
    void RemoveMetadata(string documentPath);
}