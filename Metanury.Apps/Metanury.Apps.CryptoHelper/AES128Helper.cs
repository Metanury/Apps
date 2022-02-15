using System;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace Metanury.Apps.CryptoHelper
{
    public class AES128Helper : CryptoSalt, ICryptoHelper, ITwoWay
    {
        public AES128Helper(string secretkey)
        {
            this.SecretKey = secretkey;
        }

        public string Decrypt(string keyString)
        {
            if (String.IsNullOrWhiteSpace(this.SecretKey)) throw new ArgumentNullException("SecretKey is Empty.");

            byte[] PlainText = null;
            int DecryptedCount = -1;

            using (RijndaelManaged RijndaelCipher = new RijndaelManaged())
            {
                byte[] EncryptedData = Convert.FromBase64String(this.SaltRemove(keyString.Trim()));
                byte[] Salt = Encoding.ASCII.GetBytes(this.SecretKey.Length.ToString());

                using (PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(this.SecretKey, Salt))
                using (MemoryStream memoryStream = new MemoryStream(EncryptedData))
                {
                    ICryptoTransform Decryptor = RijndaelCipher.CreateDecryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16));
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, Decryptor, CryptoStreamMode.Read))
                    {
                        PlainText = new byte[EncryptedData.Length];
                        DecryptedCount = cryptoStream.Read(PlainText, 0, PlainText.Length);
                        cryptoStream.Close();
                    }
                    memoryStream.Close();
                }
            }

            return Encoding.Unicode.GetString(PlainText, 0, DecryptedCount);
        }

        public string Encrypt(string keyString)
        {
            if (String.IsNullOrWhiteSpace(this.SecretKey)) throw new ArgumentNullException("SecretKey is Empty.");

            byte[] CipherBytes = null;

            using (RijndaelManaged RijndaelCipher = new RijndaelManaged())
            {
                byte[] PlainText = Encoding.Unicode.GetBytes(keyString);
                byte[] Salt = Encoding.ASCII.GetBytes(this.SecretKey.Length.ToString());

                using (PasswordDeriveBytes SecretBytes = new PasswordDeriveBytes(this.SecretKey, Salt))
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    ICryptoTransform Encryptor = RijndaelCipher.CreateEncryptor(SecretBytes.GetBytes(32), SecretBytes.GetBytes(16));
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, Encryptor, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(PlainText, 0, PlainText.Length);
                        cryptoStream.FlushFinalBlock();

                        CipherBytes = memoryStream.ToArray();
                        cryptoStream.Close();
                    }
                    memoryStream.Close();
                }
            }
            return this.SaltAdd(Convert.ToBase64String(CipherBytes));
        }
    }
}
