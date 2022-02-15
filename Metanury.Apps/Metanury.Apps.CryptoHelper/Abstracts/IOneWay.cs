namespace Metanury.Apps.CryptoHelper.Abstracts
{
    public interface IOneWay
    {
        string Encrypt(string keyString);
        bool ValidateCheck(string targetHash, string keyString);
    }
}
