using Metanury.Apps.Core;
using System;
using System.Linq;
using System.Text;

namespace Metanury.Apps.Data
{
    public class QueryUnit
    {
        public string FromContainer { get; set; } = string.Empty;

        public string TargetColumns { get; set; } = string.Empty;

        public string ActionMethod { get; set; } = string.Empty;

        public ActionType ActionMethodType { get; set; } = ActionType.SELECT;

        public string WhereString { get; set; } = string.Empty;

        public string ValueString { get; set; } = string.Empty;

        public string SortOrder { get; set; } = string.Empty;

        public int GetCount { get; set; } = 0;

        public enum ActionType
        {
            SELECT,
            UPDATE,
            INSERT,
            DELETE
        }

        public QueryUnit()
        {
        }

        public new string ToString()
        {
            StringBuilder builder = new StringBuilder(300);

            if (!string.IsNullOrWhiteSpace(this.FromContainer))
            {
                switch (this.ActionMethodType)
                {
                    case ActionType.SELECT:
                        builder.Append("select ");
                        if (this.GetCount > 0)
                        {
                            builder.Append($"top {this.GetCount} ");
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

                        builder.Append($"from {this.FromContainer} ");

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
                        builder.Append($"update {this.FromContainer} set");

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
                        builder.Append($"insert into {this.FromContainer} ");


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
                        builder.Append($"from {this.FromContainer} ");

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
}
