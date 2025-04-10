namespace Doco.Core.Interfaces
{
    internal interface IEncryptionService
    {
        Stream Encrypt(Stream inputStream, string key);
        Stream Decrypt(Stream inputStream, string key);
    }
}
