using System;
using System.Collections.Generic;

namespace Metanury.Apps.Paging
{
    public class PagingHelper<T> where T : class
    {
        public List<T> Data { get; set; } = new List<T>();

        public int Count { get; set; } = 0;

        public int CurrentPageNo { get; set; } = 1;

        public int CountPerPage { get; set; } = 10;

        public int CountPerPaging { get; set; } = 10;

        public string URL { get; set; } = string.Empty;

        public int SequenceNumber
        {
            get
            {
                return (Count - ((CurrentPageNo - 1) * CountPerPage));
            }
        }

        public PagingHelper()
        {
        }

        public PagingHelper(Tuple<int, List<T>> data)
        {
            this.Data = data.Item2;
            this.Count = data.Item1;
        }

        public PagingHelper(int cnt, List<T> data)
        {
            this.Data = data;
            this.Count = cnt;
        }
    }
}
