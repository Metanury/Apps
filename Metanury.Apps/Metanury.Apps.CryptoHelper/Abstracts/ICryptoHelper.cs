namespace Metanury.Apps.CryptoHelper
{
    public interface ICryptoHelper
    {
        string SaltAdd(string targetString);

        string SaltRemove(string targetString);
    }
}
