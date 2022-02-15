using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace EntityHelper
{
    public static class ExtendEntityHelper
    {
        public static PropertyInfo[] GetProperties<T>()
        {
            Type type = typeof(T);
            return type.GetProperties();
        }

        public static EntityAttribute GetEntity(this PropertyInfo property)
        {
            EntityAttribute result = new EntityAttribute();

            EntityAttribute temp;
            foreach (var attr in property.GetCustomAttributes())
            {
                try
                {
                    temp = (EntityAttribute)attr;
                    if (temp != null)
                    {
                        result = temp;
                        break;
                    }
                }
                catch
                {
                }
            }

            return result;
        }

        public static object? GetValue<T>(this T entity, string columnName) where T : IEntity
        {
            object? result = null;

            Type type = entity.GetType();
            var properties = type.GetProperties();
            EntityAttribute temp = new EntityAttribute();

            foreach (PropertyInfo property in properties)
            {
                temp = property.GetEntity();
                if (temp != null && temp.ColumnName.Equals(columnName, StringComparison.OrdinalIgnoreCase))
                {
                    result = property.GetValue(entity);
                    break;
                }
            }

            return result;
        }

        public static Dictionary<string, object> toDictionary<T>(this T entity) where T : IEntity
        {
            Dictionary<string, object> result = new Dictionary<string, object>();

            Type type = entity.GetType();
            var properties = type.GetProperties();
            EntityAttribute temp = new EntityAttribute();

            foreach (PropertyInfo property in properties)
            {
                temp = property.GetEntity();
                result.Add(temp.ColumnName, property.GetValue(temp.ColumnName));
            }

            return result;
        }

        public static Dictionary<string, object?> toDictionary<T>(this List<T> data, string columnName, string keyName) where T : IEntity, new()
        {
            Dictionary<string, object?> result = new Dictionary<string, object?>();
            T target = new T();

            Type type = target.GetType();
            PropertyInfo[] properties = type.GetProperties();
            EntityAttribute temp = new EntityAttribute();

            if (data != null && data.Count > 0)
            {
                foreach (var item in data)
                {
                    foreach (PropertyInfo property in properties)
                    {
                        temp = property.GetEntity();
                        if (temp.ColumnName.Equals(columnName, StringComparison.OrdinalIgnoreCase))
                        {
                            result.Add(keyName, item.GetValue(temp.ColumnName));
                        }
                    }
                }
            }

            return result;
        }

        public static bool Exists<T>(this T entity, string columnName) where T : IEntity
        {
            bool result = false;

            Type type = entity.GetType();
            var properties = type.GetProperties();
            EntityAttribute temp = new EntityAttribute();

            foreach (PropertyInfo property in properties)
            {
                temp = property.GetEntity();
                if (temp != null && temp.ColumnName.Equals(columnName, StringComparison.OrdinalIgnoreCase))
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        public static SqlDbType GetType<T>(this T entity, string columnName) where T : IEntity
        {
            SqlDbType result = SqlDbType.VarChar;

            Type type = entity.GetType();
            var properties = type.GetProperties();
            EntityAttribute temp = new EntityAttribute();

            foreach (PropertyInfo property in properties)
            {
                temp = property.GetEntity();
                if (temp != null && temp.ColumnName.Equals(columnName, StringComparison.OrdinalIgnoreCase))
                {
                    result = temp.Type;
                    break;
                }
            }

            return result;
        }

        public static int GetSize<T>(this T entity, string columnName) where T : IEntity
        {
            int result = -1;

            Type type = entity.GetType();
            var properties = type.GetProperties();
            EntityAttribute temp = new EntityAttribute();

            foreach (PropertyInfo property in properties)
            {
                temp = property.GetEntity();
                if (temp != null && temp.ColumnName.Equals(columnName, StringComparison.OrdinalIgnoreCase))
                {
                    result = temp.Size;
                    break;
                }
            }

            return result;
        }

        public static void SetValue<T>(this T entity, string columnName, object Value) where T : IEntity
        {
            Type type = entity.GetType();
            var properties = type.GetProperties();
            EntityAttribute temp = new EntityAttribute();

            foreach (PropertyInfo property in properties)
            {
                temp = property.GetEntity();
                if (temp != null && temp.ColumnName.Equals(columnName, StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        property.SetValue(entity, Value);
                    }
                    catch
                    {
                        if (Value.ToString().Equals("true", StringComparison.OrdinalIgnoreCase))
                        {
                            property.SetValue(entity, true);
                        }
                        else if (Value.ToString().Equals("false", StringComparison.OrdinalIgnoreCase))
                        {
                            property.SetValue(entity, false);
                        }
                    }
                    break;
                }
            }
        }

        public static List<EntityAttribute> GetEntities<T>(this T target) where T : IEntity
        {
            List<EntityAttribute> result = new List<EntityAttribute>();

            Type type = typeof(T);

            var members = type.GetMembers();
            EntityAttribute temp;
            foreach (var member in members)
            {
                foreach (var attr in member.GetCustomAttributes())
                {
                    try
                    {
                        temp = (EntityAttribute)attr;
                        if (temp != null)
                        {
                            result.Add(temp);
                        }
                    }
                    catch
                    {
                    }
                }
            }

            return result;
        }
    }
}
