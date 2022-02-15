using System.Text;

namespace Metanury.Apps.CryptoHelper
{
    public abstract class CryptoSalt
    {
        protected string SecretKey { get; set; } = string.Empty;

        public virtual string SaltAdd(string targetString)
        {
            StringBuilder builder = new StringBuilder(targetString);
            builder.Replace("=", "EvSxrTzQ");
            builder.Replace("+", "PDkcVjeDL");
            builder.Replace("/", "SkenFkkd");
            return builder.ToString();
        }

        public virtual string SaltRemove(string targetString)
        {
            StringBuilder builder = new StringBuilder(targetString);
            builder.Replace("EvSxrTzQ", "=");
            builder.Replace("PDkcVjeDL", "+");
            builder.Replace("SkenFkkd", "/");
            return builder.ToString();
        }
    }
}
