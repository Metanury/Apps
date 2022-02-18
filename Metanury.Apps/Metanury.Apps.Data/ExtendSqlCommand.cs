using Metanury.Apps.EntityHelper;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Metanury.Apps.Data
{
    public static class ExtendSqlCommand
    {

        #region [ Execute ]
        public static DataTable ExecuteTable(this SqlCommand cmd)
        {
            var result = new DataTable();

            using (var adp = new SqlDataAdapter(cmd))
            {
                adp.Fill(result);
            }

            return result;
        }

        public static List<DataTable> ExecuteTables(this SqlCommand cmd)
        {
            var result = new List<DataTable>();

            var dbset = cmd.ExecuteDataSet();
            if (dbset != null && dbset.Tables != null && dbset.Tables.Count > 0)
            {
                foreach (DataTable tbl in dbset.Tables)
                {
                    result.Add(tbl);
                }
            }

            return result;
        }

        public static DataSet ExecuteDataSet(this SqlCommand cmd)
        {
            var result = new DataSet();

            using (var adp = new SqlDataAdapter(cmd))
            {
                adp.Fill(result);
            }

            return result;
        }

        public static List<T> ExecuteList<T>(this SqlCommand cmd) where T : new()
        {
            var result = new List<T>();

            DataTable dt = cmd.ExecuteTable();
            if (dt != null && dt.Rows != null && dt.Rows.Count > 0)
            {
                result = ConvertEntity.ColumnToEntity<T>(dt);
            }

            return result;
        }

        #endregion [ Execute ]


        #region [ Parameter ]

        public static void AddParameterOutput(this SqlCommand cmd, string name, SqlDbType type, int size = -1)
        {
            cmd.Parameters.Add(name, type, size);
            cmd.Parameters[name].Direction = ParameterDirection.Output;
        }

        public static void AddParameterInput(this SqlCommand cmd, string name, SqlDbType type, object value, int size = -1)
        {
            cmd.Parameters.Add(name, type, size);
            cmd.Parameters[name].Value = value;
        }

        public static object GetValue(this SqlCommand cmd, string name)
        {
            return cmd.Parameters[name].Value;
        }

        #endregion [ Parameter ]

    }
}
