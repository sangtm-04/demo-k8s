using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Demo.WebApp.Common
{
    public class Utility
    {
        public static object ConvertType(string value, MySqlDbType sqlDbType)
        {
            object result;
            Type type = GetClrType(sqlDbType);

            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            try
            {
                var converter = TypeDescriptor.GetConverter(type);
                result = converter.ConvertFromString(value);

            }
            catch (Exception exception)
            {

                // Log this exception if required.
                throw new InvalidCastException(string.Format("Unable to cast the {0} to type {1}", value, "newType", exception));
            }
            return result;
        }

        public static Type GetClrType(MySqlDbType sqlType)
        {
            switch (sqlType)
            {
                case MySqlDbType.UInt16:
                case MySqlDbType.UInt24:
                case MySqlDbType.UInt32:
                case MySqlDbType.UInt64:
                    return typeof(long?);

                case MySqlDbType.Binary:
                //case MySqlDbType.Image:
                case MySqlDbType.Timestamp:
                case MySqlDbType.VarBinary:
                    return typeof(byte[]);

                case MySqlDbType.Bit:
                    return typeof(bool?);

                case MySqlDbType.VarChar:
                case MySqlDbType.LongText:
                case MySqlDbType.String:
                case MySqlDbType.Text:
                    return typeof(string);

                case MySqlDbType.DateTime:
                case MySqlDbType.Date:
                case MySqlDbType.Time:
                    return typeof(DateTime?);

                case MySqlDbType.Decimal:
                    return typeof(decimal?);

                case MySqlDbType.Float:
                    return typeof(double?);

                case MySqlDbType.Int32:
                case MySqlDbType.Int64:
                    return typeof(int?);

                case MySqlDbType.Int16:
                    return typeof(short?);

                default:
                    throw new ArgumentOutOfRangeException("sqlType");
            }
        }

        public static string GetEntityName<T>()
        {
            var entity = Activator.CreateInstance<T>();
            return entity.GetType().Name;
        }
    }
}
