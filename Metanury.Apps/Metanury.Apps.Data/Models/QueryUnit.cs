using Metanury.Apps.Core;
using Metanury.Apps.EntityHelper;
using System;
using System.Linq;
using System.Text;

namespace Metanury.Apps.Data
{
    public class QueryUnit
    {
        public string FromContainer { get; set; } = string.Empty;

        public string TargetColumns { get; set; } = string.Empty;

        public ActionType ActionMethodType { get; set; } = ActionType.SELECT;

        public string WhereString { get; set; } = string.Empty;

        public string ValueString { get; set; } = string.Empty;

        public string SortOrder { get; set; } = string.Empty;

        public int GetRecordCount { get; set; } = 0;

        public IQueryList QueryList { get; set; }

        public enum ActionType
        {
            SELECT,
            UPDATE,
            INSERT,
            DELETE,
            LIST,
            COUNT
        }

        public QueryUnit()
        {
        }

        public QueryUnit GetRows(int cnt = 10)
        {
            this.GetRecordCount = cnt;
            return this;
        }

        public QueryUnit SetTable(string table)
        {
            this.FromContainer = table;
            return this;
        }

        public QueryUnit SetValues(string values)
        {
            this.ValueString = values;
            return this;
        }

        public QueryUnit SetColumns(string columns)
        {
            this.TargetColumns = columns;
            return this;
        }

        public QueryUnit SetWhere(string where)
        {
            this.WhereString = where;
            return this;
        }

        public QueryUnit AddWhere(string where)
        {
            this.WhereString = (string.IsNullOrWhiteSpace(this.WhereString)) ? where : $"{this.WhereString} and {where}";
            return this;
        }

        public QueryUnit SetSort(string order)
        {
            this.SortOrder = order;
            return this;
        }

        public QueryUnit AddSort(string order)
        {
            this.SortOrder = (string.IsNullOrWhiteSpace(this.SortOrder)) ? order : $"{this.SortOrder}, {order}";
            return this;
        }

        public new string ToString()
        {
            StringBuilder builder = new StringBuilder(300);

            if (!string.IsNullOrWhiteSpace(this.FromContainer))
            {
                switch (this.ActionMethodType)
                {
                    case ActionType.LIST:
                        if (QueryList == null)
                        {
                            throw new ArgumentNullException(nameof(this.QueryList));
                        }
                        else if (string.IsNullOrWhiteSpace(this.SortOrder))
                        {
                            throw new ArgumentNullException(nameof(this.SortOrder));
                        }
                        else
                        {
                            builder.Append($"SELECT TOP ({QueryList.PageSize}) resultTable.* FROM");
                            builder.Append($"( SELECT TOP ({QueryList.PageSize * QueryList.CurPage}) ROW_NUMBER () OVER ");
                            builder.Append($" (ORDER BY { this.SortOrder }) AS rownumber, *");
                            builder.Append($" FROM [{this.FromContainer}] with (nolock) ");
                            if (!string.IsNullOrWhiteSpace(this.WhereString))
                            {
                                builder.Append(" where " + this.WhereString);
                            }
                            builder.Append($" ORDER BY { this.SortOrder }");
                            builder.Append(") AS resultTable");
                            builder.Append($" WHERE rownumber > {(QueryList.CurPage - 1) * QueryList.PageSize}");
                        }
                        break;
                    case ActionType.COUNT:
                        builder.Append("select count(1) ");
                        builder.Append($"from [{this.FromContainer}] with (nolock) ");

                        if (!string.IsNullOrWhiteSpace(this.WhereString))
                        {
                            builder.Append($"where {this.WhereString} ");
                        }
                        break;
                    case ActionType.SELECT:
                        builder.Append("select ");
                        if (this.GetRecordCount > 0)
                        {
                            builder.Append($"top {this.GetRecordCount} ");
                        }
                        if (!string.IsNullOrWhiteSpace(this.TargetColumns))
                        {
                            builder.Append(this.TargetColumns);
                            builder.Append(" ");
                        }
                        else
                        {
                            builder.Append("* ");
                        }

                        builder.Append($"from [{this.FromContainer}] with (nolock) ");

                        if (!string.IsNullOrWhiteSpace(this.WhereString))
                        {
                            builder.Append($"where {this.WhereString} ");
                        }
                        if (!string.IsNullOrWhiteSpace(this.SortOrder))
                        {
                            builder.Append($"order by {this.SortOrder}");
                        }
                        break;
                    case ActionType.UPDATE:
                        builder.Append($"update [{this.FromContainer}] set");

                        foreach(var update in (from a in this.TargetColumns.SplitWithSeq(',') join
                             b in this.ValueString.SplitWithSeq(',') on a.Key equals b.Key
                             select new { Column = a.Value, Value = b.Value, idx = a.Key }))
                        {
                            if (update.idx > 0) builder.Append(",");

                            builder.Append($"{update.Column}={update.Value}");
                        }

                        if (!string.IsNullOrWhiteSpace(this.TargetColumns))
                        {
                            builder.Append(this.TargetColumns);
                            builder.Append(" ");
                        }
                        else
                        {
                            builder.Append("* ");
                        }

                        if (!string.IsNullOrWhiteSpace(this.WhereString))
                        {
                            builder.Append($"where {this.WhereString} ");
                        }
                        break;
                    case ActionType.INSERT:
                        builder.Append($"insert into [{this.FromContainer}] ");


                        if (!string.IsNullOrWhiteSpace(this.TargetColumns))
                        {
                            builder.Append($"({this.TargetColumns}) ");
                        }

                        if (!string.IsNullOrWhiteSpace(this.ValueString))
                        {
                            builder.Append($"values ({this.ValueString})");
                        }
                        else
                        {
                            throw new ArgumentNullException(nameof(this.ValueString));
                        }
                        break;
                    case ActionType.DELETE:
                        builder.Append("delete ");
                        builder.Append($"from [{this.FromContainer}] ");

                        if (!string.IsNullOrWhiteSpace(this.WhereString))
                        {
                            builder.Append($"where {this.WhereString} ");
                        }
                        break;
                }
            }
            else
            {
                throw new ArgumentNullException(nameof(this.FromContainer));
            }

            return builder.ToString();
        }
    }

    public static class ExtendQueryUnit
    {
        public static QueryUnit Select(this IEntity entity, Func<QueryUnit, QueryUnit> func = null)
        {
            var query = new QueryUnit();
            query.FromContainer = entity.TableName;
            query.ActionMethodType = QueryUnit.ActionType.SELECT;
            if (func != null)
            {
                return func.Invoke(query);
            }
            return query;
        }

        public static QueryUnit Update(this IEntity entity, Func<QueryUnit, QueryUnit> func = null)
        {
            var query = new QueryUnit();
            query.FromContainer = entity.TableName;
            query.ActionMethodType = QueryUnit.ActionType.UPDATE;
            if (func != null)
            {
                return func.Invoke(query);
            }
            return query;
        }

        public static QueryUnit Insert(this IEntity entity, Func<QueryUnit, QueryUnit> func = null)
        {
            var query = new QueryUnit();
            query.FromContainer = entity.TableName;
            query.ActionMethodType = QueryUnit.ActionType.INSERT;
            if (func != null)
            {
                return func.Invoke(query);
            }
            return query;
        }

        public static QueryUnit Delete(this IEntity entity, Func<QueryUnit, QueryUnit> func = null)
        {
            var query = new QueryUnit();
            query.FromContainer = entity.TableName;
            query.ActionMethodType = QueryUnit.ActionType.DELETE;
            if (func != null)
            {
                return func.Invoke(query);
            }
            return query;
        }

        public static QueryUnit List(this IEntity entity, Func<QueryUnit, QueryUnit> func = null)
        {
            var query = new QueryUnit();
            query.FromContainer = entity.TableName;
            query.ActionMethodType = QueryUnit.ActionType.LIST;
            if (func != null)
            {
                return func.Invoke(query);
            }
            return query;
        }

        public static QueryUnit Count(this IEntity entity, Func<QueryUnit, QueryUnit> func = null)
        {
            var query = new QueryUnit();
            query.FromContainer = entity.TableName;
            query.ActionMethodType = QueryUnit.ActionType.COUNT;
            if (func != null)
            {
                return func.Invoke(query);
            }
            return query;
        }
    }
}
