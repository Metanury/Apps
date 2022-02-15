using Metanury.Apps.CryptoHelper.Abstracts;
using System.Security.Cryptography;
using System.Text;

namespace Metanury.Apps.CryptoHelper
{
    public class SHA256Helper : IOneWay
    {
        public string Encrypt(string keyString)
        {
            StringBuilder result = new StringBuilder(64);
            SHA256 mySHA256 = SHA256Managed.Create();
            byte[] temp = Encoding.Default.GetBytes(keyString);
            byte[] hashValue = mySHA256.ComputeHash(temp);
            int i = 0;
            for (i = 0; i < hashValue.Length; i++)
            {
                result.AppendFormat("{0:X2}", hashValue[i]);
            }
            return result.ToString();
        }

        public bool ValidateCheck(string targetHash, string keyString)
        {
            if (Encrypt(keyString) == targetHash)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
