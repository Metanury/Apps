using System.Collections.Generic;

namespace Metanury.Apps.Core
{
    public class SequenceObject
    {
        public int Key { get; set; } = 0;

        public string Value { get; set; } = string.Empty;

        public SequenceObject(int key, string value)
        {
            this.Key = key;
            this.Value = value;
        }
    }

    public static class SequenceObjectExtensions
    {
        public static List<SequenceObject> SplitWithSeq(this string str, char key)
        {
            var result = new List<SequenceObject>();

            int num = 0;
            foreach(string on in str.Split(key))
            {
                result.Add(new SequenceObject(num++, on));
            }

            return result;
        }
    }
}
