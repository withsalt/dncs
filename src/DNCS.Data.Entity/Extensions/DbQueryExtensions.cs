using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Data;

using Microsoft.EntityFrameworkCore;

namespace DNCS.Data.Entity.Extensions
{
    public static class DbQueryExtensions
    {

        #region Load

        public static DbCommand LoadStoredProcedure(this CustumDbContext context, string storedProcName)
        {
            var cmd = context.Database.GetDbConnection().CreateCommand();
            cmd.CommandText = storedProcName;
            cmd.CommandType = CommandType.StoredProcedure;
            return cmd;
        }

        public static DbCommand LoadSqlQuery(this CustumDbContext context, string sql)
        {
            var cmd = context.Database.GetDbConnection().CreateCommand();
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            return cmd;
        }

        #endregion

        public static DbCommand SqlParam(this DbCommand cmd, string paramName, object paramValue)
        {
            if (string.IsNullOrEmpty(cmd.CommandText))
                throw new InvalidOperationException(
                  "Call LoadStoredProc before using this method");
            var param = cmd.CreateParameter();
            param.ParameterName = paramName;
            param.Value = paramValue;
            cmd.Parameters.Add(param);
            return cmd;
        }

        public static async Task<List<T>> ExecuteStoredProcedureAsync<T>(this DbCommand command) where T : class, new()
        {
            using (command)
            {
                if (command.Connection.State == ConnectionState.Closed)
                    command.Connection.Open();
                try
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        return reader.MapToList<T>();
                    }
                }
                catch (Exception e)
                {
                    throw (e);
                }
                finally
                {
                    command.Connection.Close();
                }
            }
        }

        public static async Task<T> ExecuteEntityQueryAsync<T>(this DbCommand command) where T : class, new()
        {
            using (command)
            {
                if (command.Connection.State == ConnectionState.Closed)
                    command.Connection.Open();
                try
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        return reader.MapToEntity<T>();
                    }
                }
                catch (Exception e)
                {
                    throw (e);
                }
                finally
                {
                    command.Connection.Close();
                }
            }
        }

        public static async Task<List<T>> ExecuteListQueryAsync<T>(this DbCommand command) where T : class, new()
        {
            using (command)
            {
                if (command.Connection.State == ConnectionState.Closed)
                    command.Connection.Open();
                try
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        return reader.MapToList<T>();
                    }
                }
                catch (Exception e)
                {
                    throw (e);
                }
                finally
                {
                    command.Connection.Close();
                }
            }
        }

        #region Map
        private static List<T> MapToList<T>(this DbDataReader dr) where T : new()
        {
            if (dr == null || !dr.HasRows)
            {
                return default;
            }

            var entity = typeof(T);
            var entities = new List<T>();
            var props = entity.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            var propDict = props.ToDictionary(m => m.Name.ToUpper(), m => m);

            while (dr.Read())
            {
                T obj = new T();
                for (int index = 0; index < dr.FieldCount; index++)
                {
                    if (propDict.ContainsKey(dr.GetName(index).ToUpper()))
                    {
                        var info = propDict[dr.GetName(index).ToUpper()];
                        if (info != null && info.CanWrite)
                        {
                            var val = dr.GetValue(index);
                            if (val == null)
                            {
                                continue;
                            }
                            if (val == DBNull.Value)
                            {
                                info.SetValue(obj, null);
                            }
                            else
                            {
                                Type nullableType = Nullable.GetUnderlyingType(info.PropertyType);
                                bool isNullableType = nullableType != null;
                                object convertVal;
                                if (nullableType != null)
                                {
                                    convertVal = Convert.ChangeType(val, nullableType);
                                }
                                else
                                {
                                    convertVal = Convert.ChangeType(val, info.PropertyType);
                                }
                                info.SetValue(obj, convertVal);
                            }
                        }
                    }
                }
                entities.Add(obj);
            }
            return entities;
        }

        private static T MapToEntity<T>(this DbDataReader dr) where T : new()
        {
            if (dr == null || !dr.HasRows)
            {
                return default;
            }

            var entity = typeof(T);
            if (entity.IsValueType)
            {
                while (dr.Read())
                {
                    return (T)dr.GetValue(0);
                }
            }

            var props = entity.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            var propDict = props.ToDictionary(m => m.Name.ToUpper(), m => m);

            if (dr.Read())
            {
                var obj = new T();
                for (int index = 0; index < dr.FieldCount; index++)
                {
                    if (propDict.ContainsKey(dr.GetName(index).ToUpper()))
                    {
                        var info = propDict[dr.GetName(index).ToUpper()];
                        if (info != null && info.CanWrite)
                        {
                            var val = dr.GetValue(index);
                            if (val == null)
                            {
                                continue;
                            }
                            if (val == DBNull.Value)
                            {
                                info.SetValue(obj, null);
                            }
                            else
                            {
                                Type nullableType = Nullable.GetUnderlyingType(info.PropertyType);
                                bool isNullableType = nullableType != null;
                                object convertVal;
                                if (nullableType != null)
                                {
                                    convertVal = Convert.ChangeType(val, nullableType);
                                }
                                else
                                {
                                    convertVal = Convert.ChangeType(val, info.PropertyType);
                                }
                                info.SetValue(obj, convertVal);
                            }
                        }
                    }
                }
                return obj;
            }
            else
            {
                return default;
            }
        }

        private static bool IsInteger(ValueType value)
        {
            return (value is SByte || value is Int16 || value is Int32
                    || value is Int64 || value is Byte || value is UInt16
                    || value is UInt32 || value is UInt64
                    || value is BigInteger);
        }

        private static bool IsFloat(ValueType value)
        {
            return (value is float | value is double | value is Decimal);
        }

        private static bool IsNumeric(ValueType value)
        {
            return (value is Byte ||
                    value is Int16 ||
                    value is Int32 ||
                    value is Int64 ||
                    value is SByte ||
                    value is UInt16 ||
                    value is UInt32 ||
                    value is UInt64 ||
                    value is BigInteger ||
                    value is Decimal ||
                    value is Double ||
                    value is Single);
        }

        #endregion
    }
}
