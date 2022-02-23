using System;

namespace Metanury.Apps.Core
{
    public class RandomHelper
    {
        private static Random rnd;

        static RandomHelper()
        {
            rnd = new Random();
        }

        public static string RandomString(int size)
        {
            Random random = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
            const string strPool = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            char[] chRandom = new char[size];
            for (int i = 0; i < size; i++)
            {
                chRandom[i] = strPool[random.Next(strPool.Length)];
            }
            string strRet = new String(chRandom);
            return strRet;
        }

        public static int RandomInt(int size)
        {
            return rnd.Next(size);
        }
    }
}
