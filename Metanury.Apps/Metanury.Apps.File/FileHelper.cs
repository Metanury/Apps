using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Metanury.Apps.File
{
    public class FileHelper
    {
        /// <summary>
        /// 파일 읽기
        /// </summary>
        /// <param name="fileURL">경로</param>
        /// <param name="EncMode">문자열형식</param>
        /// <returns>String</returns>
        public static string ReadFile(string fileURL, Encoding EncMode)
        {
            StringBuilder result = new StringBuilder();

            try
            {
                if (!(String.IsNullOrEmpty(fileURL)))
                {
                    FileInfo fi = new FileInfo(fileURL);
                    if (fi.Exists)
                    {
                        using (StreamReader reader = new StreamReader(fileURL, EncMode))
                        {
                            result.Append(reader.ReadToEnd());
                            reader.Close();
                        }
                    }
                    fi = null;
                }
            }
            catch (Exception ex)
            {
                result.Clear();
                result.Append(ex.Message.ToString());
            }

            return result.ToString();
        }

        /// <summary>
        /// 파일 읽기
        /// </summary>
        /// <param name="fileURL">경로</param>
        /// <param name="EncMode">문자열형식</param>
        /// <returns>String</returns>
        public static Task<string> ReadFileAsync(string fileURL, Encoding EncMode)
        {
            return Task.Factory.StartNew(() => ReadFile(fileURL, EncMode));
        }

        /// <summary>
        /// 파일쓰기
        /// </summary>
        /// <param name="fileURL">파일경로</param>
        /// <param name="bodyText">파일내용</param>
        /// <param name="EncMode">Encoding Type</param>
        /// <param name="IsAppend">Over Write 여부</param>
        /// <returns></returns>
        public static bool WriteFile(string fileURL, string bodyText, Encoding EncMode, bool IsAppend = false)
        {
            bool result = false;

            try
            {
                if (!String.IsNullOrWhiteSpace(fileURL))
                {
                    using (StreamWriter writer = new StreamWriter(fileURL, IsAppend, EncMode))
                    {
                        writer.WriteLine(bodyText);
                        writer.Close();
                        result = true;
                    }
                }
            }
            catch
            {
                result = false;
            }

            return result;
        }

        public static Task<bool> WriteFileAsync(string fileURL, string bodyText, Encoding EncMode, bool IsAppend = false)
        {
            return Task.Factory.StartNew(() => WriteFile(fileURL, bodyText, EncMode, IsAppend));
        }
    }
}
