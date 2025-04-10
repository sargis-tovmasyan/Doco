namespace Doco.Core.Interfaces;

using System.IO;
using System.Threading.Tasks;

public interface IStorageProvider
{
    Task<Stream> ReadAsync(string path);
    Task WriteAsync(string path, Stream content);
    Task DeleteAsync(string path);
    bool Exists(string path);
}