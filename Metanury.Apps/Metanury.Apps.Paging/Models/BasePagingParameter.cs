using Metanury.Apps.Data;
using System.Text;

namespace Metanury.Apps.Paging
{
    public class BasePagingParameter : IQueryList
    {
        public int PageSize { get; set; } = 10;
        public int CurPage { get; set; } = 1;

        public string Keyfield { get; set; } = string.Empty;

        public string Keyword { get; set; } = string.Empty;

        public BasePagingParameter()
        {
        }

        public virtual string Serialize()
        {
            return $"Keyfield={this.Keyfield}&Keyword={this.Keyword}";
        }

        public virtual string toWhereString()
        {
            return (this.IsExistsWehre()) ? $"{this.Keyfield} like '%{this.Keyword}%'" : string.Empty;
        }

        public virtual bool IsExistsWehre()
        {
            return (!string.IsNullOrWhiteSpace(this.Keyfield) && !string.IsNullOrWhiteSpace(this.Keyword)) ? true : false;
        }
    }
}
