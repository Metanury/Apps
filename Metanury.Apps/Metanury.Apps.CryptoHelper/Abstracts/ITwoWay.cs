namespace Metanury.Apps.CryptoHelper
{
    public interface ITwoWay
    {
        string Encrypt(string keyString);
        string Decrypt(string keyString);
    }
}
