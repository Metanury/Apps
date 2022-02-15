namespace Metanury.Apps.CryptoHelper
{
    public interface IOneWay
    {
        string Encrypt(string keyString);
        bool ValidateCheck(string targetHash, string keyString);
    }
}
