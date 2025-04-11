   namespace Doco.Core.Interfaces;

   public interface IMetaDatabase
   {
       IDictionary<string, string> GetMetadata(string documentPath);
       void SetMetadata(string documentPath, IDictionary<string, string> metadata);
       void RemoveMetadata(string documentPath, IEnumerable<string> keys);
   }