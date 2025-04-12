   using Doco.Core.Models;

   namespace Doco.Core.Interfaces;

   public interface IMetaDatabase
   {
       DocumentMetadata GetMetadata(string documentPath);
       void SetMetadata(string documentPath, DocumentMetadata documentMetadata);
       bool RemoveMetadata(string documentPath);
   }