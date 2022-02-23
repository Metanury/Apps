namespace Metanury.Apps.Data
{
    public interface IQueryList
    {
        int PageSize { get; set; }

        int CurPage { get; set; }

        string Serialize();

        string toWhereString();

        bool IsExistsWehre();
    }
}
