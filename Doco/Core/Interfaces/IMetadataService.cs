namespace Doco.Core.Interfaces;

using System.Collections.Generic;
using System.Threading.Tasks;

public interface IMetadataService
{
    IDictionary<string, string> GetMetadataAsync(string documentPath);
    void SetMetadataAsync(string documentPath, IDictionary<string, string> metadata);
    void RemoveMetadataAsync(string documentPath, IEnumerable<string> keys);
}