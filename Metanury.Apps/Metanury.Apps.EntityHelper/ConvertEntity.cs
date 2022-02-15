using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Metanury.Apps.EntityHelper
{
    public class ConvertEntity
    {
        public static List<T> ColumnToEntity<T>(DataTable data) where T : new()
        {
            var result = new List<T>();
            var properties = EntityHelper.GetProperties<T>();
            var columns = data.Columns.Cast<DataColumn>().ToList();
            T item;
            foreach (DataRow row in data.Rows)
            {
                item = new T();
                DataColumn column;
                foreach (var property in properties)
                {
                    try
                    {
                        column = columns.Find(x => x.ColumnName == property.Name);
                        if (column != null && row[property.Name] != null && row[property.Name] != DBNull.Value)
                        {
                            property.SetValue(item, row[property.Name], null);
                        }
                    }
                    catch
                    {

                    }
                }

                result.Add(item);
            }

            return result;
        }

        public static List<T> PropertyToEntity<T>(DataTable data) where T : new()
        {
            var result = new List<T>();
            var properties = EntityHelper.GetProperties<T>();
            var columns = data.Columns.Cast<DataColumn>().ToList();
            T item;
            foreach (var property in properties)
            {
                item = new T();
                DataColumn column = null;
                foreach (DataRow row in data.Rows)
                {
                    try
                    {
                        column = columns.Find(x => x.ColumnName == property.Name);
                        if (column != null && row[property.Name] != null && row[property.Name] != DBNull.Value)
                        {
                            property.SetValue(item, row[property.Name], null);
                        }
                    }
                    catch
                    {

                    }
                }

                result.Add(item);
            }

            return result;
        }
    }
}
