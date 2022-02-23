using Metanury.Apps.Data;
using Metanury.Apps.EntityHelper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Metanury.Apps.Paging
{
    public class PagingHelper<T> where T : IEntity
    {
        public List<T> Data { get; set; } = new List<T>();

        public int TotalRecordCount { get; set; } = 0;

        public int CountPerPaging { get; set; } = 10;

        public string URL { get; set; } = string.Empty;

        public IQueryList Parameter { get; set; }

        public int SequenceNumber
        {
            get
            {
                return (TotalRecordCount - ((Parameter.CurPage - 1) * Parameter.PageSize));
            }
        }

        protected int lastPage { get; set; } = 1;

        public int LastPage
        {
            get
            {
                return this.lastPage;
            }
        }

        public PagingHelper()
        {
        }

        public PagingHelper(Tuple<int, List<T>> data)
        {
            this.Data = data.Item2;
            this.TotalRecordCount = data.Item1;
        }

        public PagingHelper(int cnt, List<T> data)
        {
            this.Data = data;
            this.TotalRecordCount = cnt;
        }

        public virtual List<int> GetPaging()
        {
            List<int> result = new List<int>();

            if (TotalRecordCount > Parameter.PageSize)
            {
                int st = 1;
                int ed = this.CountPerPaging;

                this.lastPage = this.TotalRecordCount / Parameter.PageSize;
                int tmp = this.TotalRecordCount % Parameter.PageSize;
                if (tmp > 0)
                {
                    this.lastPage++;
                }

                if (Parameter.CurPage > this.CountPerPaging)
                {
                    st = Convert.ToInt32(Parameter.CurPage / this.CountPerPaging) * this.CountPerPaging + 1;
                    ed = st + (this.CountPerPaging - 1);
                }

                for (int i = st; i <= ed; i++)
                {
                    if (i > this.lastPage) break;
                    result.Add(i);
                }
            }

            if (result.Count == 0)
            {
                result.Add(1);
            }

            return result;
        }

        public virtual string GetURL(int pageNum)
        {
            StringBuilder builder = new StringBuilder(100);
            if (!string.IsNullOrWhiteSpace(this.URL))
            {
                builder.Append(URL);
                if (this.URL.IndexOf("?") > -1)
                {
                    builder.Append("&");
                }
                else
                {
                    builder.Append("?");
                }
                builder.Append($"CurPage={pageNum}");
            }
            return builder.ToString();
        }

        public virtual string CreateStringByHtml()
        {
            StringBuilder builder = new StringBuilder(200);
            builder.AppendLine("<ul>");
            builder.AppendLine($"<li><a href=\"{this.GetURL(1)}\"&{Parameter.Serialize()}\">&lt;&lt;</a></li>");
            builder.AppendLine($"<li><a href=\"{this.GetURL(this.PreviousPage)}\"&{Parameter.Serialize()}\">&lt;&lt;</a></li>");
            foreach (int p in this.GetPaging())
            {
                builder.AppendLine($"<li class=\"{this.PageCheck(p)}\"><a href=\"{this.GetURL(p)}\"&{Parameter.Serialize()}\">{p}</a></li>");
            }
            builder.AppendLine($"<li><a href=\"{this.GetURL(this.NextPage)}\"&{Parameter.Serialize()}\">&gt;&gt;</a></li>");
            builder.AppendLine($"<li><a href=\"{this.GetURL(this.LastPage)}\"&{Parameter.Serialize()}\">&gt;&gt;</a></li>");
            builder.AppendLine("</ul>");
            return builder.ToString();
        }

        public virtual string CreateStringByScript(string funcName)
        {
            StringBuilder builder = new StringBuilder(200);
            builder.AppendLine("<ul>");
            builder.AppendLine($"<li><a href=\"javascript:{funcName}('{this.GetURL(1)}\"&{Parameter.Serialize()}');\">&lt;&lt;</a></li>");
            builder.AppendLine($"<li><a href=\"javascript:{funcName}('{this.GetURL(this.PreviousPage)}\"&{Parameter.Serialize()}');\">&lt;&lt;</a></li>");
            foreach (int p in this.GetPaging())
            {
                builder.AppendLine($"<li class=\"{this.PageCheck(p)}\"><a href=\"javascript:{funcName}('{this.GetURL(p)}\"&{Parameter.Serialize()}');\">{p}</a></li>");
            }
            builder.AppendLine($"<li><a href=\"javascript:{funcName}('{this.GetURL(this.NextPage)}\"&{Parameter.Serialize()}');\">&gt;&gt;</a></li>");
            builder.AppendLine($"<li><a href=\"javascript:{funcName}('{this.GetURL(this.LastPage)}\"&{Parameter.Serialize()}');\">&gt;&gt;</a></li>");
            builder.AppendLine("</ul>");
            return builder.ToString();
        }

        private string PageCheck(int num)
        {
            return (num == Parameter.CurPage) ? "active" : string.Empty;
        }

        public int PreviousPage
        {
            get
            {
                if (this.TotalRecordCount > 0)
                {
                    int tmp = this.GetPaging()[0];
                    if (tmp > Parameter.PageSize)
                    {
                        tmp = (Convert.ToInt32(tmp / this.CountPerPaging) * this.CountPerPaging) + 1;
                        tmp = tmp - this.CountPerPaging;
                    }
                    else
                    {
                        tmp = 1;
                    }
                    return tmp;
                }
                else
                {
                    return 1;
                }
            }
        }

        public int NextPage
        {
            get
            {
                if (this.TotalRecordCount > 0)
                {
                    int tmp = this.GetPaging()[this.GetPaging().Count - 1];
                    if (tmp < this.LastPage)
                    {
                        tmp = (Convert.ToInt32(tmp / this.CountPerPaging) * this.CountPerPaging) + 1;
                        if (tmp > this.LastPage)
                        {
                            tmp = this.LastPage;
                        }
                    }
                    else
                    {
                        tmp = this.LastPage;
                    }
                    return tmp;
                }
                else
                {
                    return this.LastPage;
                }
            }
        }
    }
}
