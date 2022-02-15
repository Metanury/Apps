namespace Metanury.Apps.EntityHelper
{
    public interface IEntity
    {
        string TableName { get; set; }
        string TargetColumn { get; set; }
    }
}
