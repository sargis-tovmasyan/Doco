namespace Doco.Core.Interfaces;

using System.Collections.Generic;
using System.Threading.Tasks;

public interface IMetadataService
{
    Task<IDictionary<string, string>> GetMetadataAsync(string documentPath);
    Task SetMetadataAsync(string documentPath, IDictionary<string, string> metadata);
    Task RemoveMetadataAsync(string documentPath, IEnumerable<string> keys);
}