namespace Metanury.Apps.CryptoHelper.Abstracts
{
    public interface ITwoWay
    {
        string Encrypt(string keyString);
        string Decrypt(string keyString);
    }
}
