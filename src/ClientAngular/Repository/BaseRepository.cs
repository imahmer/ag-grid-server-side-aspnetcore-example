using ClientAngular.Common;
using ClientAngular.Interfaces;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace ClientAngular.Repository
{
    public class BaseRepository<T> : IBaseRepository<T>
    {
        private string _connectionString = "";
        internal IDbConnection Connection
        {
            get
            {
                return new SqlConnection(_connectionString);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        public BaseRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(long id)
        {
            var result = SqlMapper.Execute(Connection, GetSoftDeleteQuery(), new { ID = id });
            return result == 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        public bool DeleteByColumnCriteria(T obj, object columns)
        {
            var result = SqlMapper.Execute(Connection, GetSoftDeleteQueryWithModificationInfo(columns), obj);
            return result == 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        public bool DeleteByColumnName(object columns)
        {
            var result = SqlMapper.Execute(Connection, GetSoftDeleteQueryForColumn(columns), columns);
            return result > 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool DeleteModified(T obj)
        {
            var result = SqlMapper.Execute(Connection, GetSoftDeleteQueryWithModificationInfo(), obj);
            return result == 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> GetAll()
        {
            var result = SqlMapper.Query<T>(Connection, GetAllQuery());
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        public IEnumerable<T> GetAllByColumns(object columns)
        {
            var result = SqlMapper.Query<T>(Connection, GetAllByColumnsQuery(columns), columns);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        public T GetByColumns(object columns)
        {
            var result = SqlMapper.QueryFirstOrDefault<T>(Connection, GetByColumnsQuery(columns), columns);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetById(long id)
        {
            var result = SqlMapper.QueryFirstOrDefault<T>(Connection, GetByIdQuery(), new { ID = id });
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public long GetCount()
        {
            var result = SqlMapper.QueryFirst<long>(Connection, GetCountQuery());
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        public long GetCountByColumns(object columns)
        {
            var result = SqlMapper.QueryFirst<long>(Connection, GetCountByColumnsQuery(columns));
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public IEnumerable<T> GetDistinctColumnByName(string columnName)
        {
            var result = SqlMapper.Query<T>(Connection, GetDistinctColumnQuery(columnName), columnName);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="whereClause"></param>
        /// <returns></returns>
        public IEnumerable<T> GetDistinctColumnByName(string columns, object whereClause)
        {
            var result = SqlMapper.Query(Connection, GetDistinctColumnQuery(columns, whereClause), whereClause);
            return (IEnumerable<T>)result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool HardDelete(T obj)
        {
            var result = SqlMapper.Execute(Connection, GetHardDeleteQuery(), obj);
            return result == 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="columnCriteria"></param>
        /// <returns></returns>
        public bool HardDelete(object columnCriteria)
        {
            var result = SqlMapper.Execute(Connection, GetHardDeleteQuery(columnCriteria), columnCriteria);
            return result == 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public long Insert(T obj)
        {
            var result = SqlMapper.Query(Connection, GetInsertQuery(obj), obj);
            long newRecordId = 1;
            if (result.FirstOrDefault() != null)
            {
                newRecordId = result.FirstOrDefault().ID;
            }
            return newRecordId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="dbTransaction"></param>
        /// <returns></returns>
        public long Insert(T obj, DbTransaction dbTransaction)
        {
            var result = SqlMapper.Query(Connection, GetInsertQuery(obj), obj, dbTransaction);
            long newRecordId = 1;
            if (result.FirstOrDefault() != null)
            {
                newRecordId = result.FirstOrDefault().ID;
            }
            return newRecordId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsExists(long id)
        {
            bool isExists = SqlMapper.ExecuteScalar<bool>(Connection, GetIsExistsByIdQuery(), new { ID = id });
            return isExists;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="whereColumns"></param>
        /// <param name="idNotEqualTo"></param>
        /// <returns></returns>
        public bool IsExists(object whereColumns, long idNotEqualTo = 0)
        {
            bool isExists = SqlMapper.ExecuteScalar<bool>(Connection, GetIsExistsByPKColumnsQuery(whereColumns, idNotEqualTo), whereColumns);
            return isExists;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Restore(long id)
        {
            var result = SqlMapper.Execute(Connection, GetRestoreQuery(), new { ID = id });
            return result == 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool RestoreModified(T obj)
        {
            var result = SqlMapper.Execute(Connection, GetRestoreModifiedQuery(), obj);
            return result == 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Update(T obj)
        {
            var result = SqlMapper.Execute(Connection, GetUpdateQuery(obj), obj);
            return result == 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Upsert(T obj)
        {
            var result = SqlMapper.Execute(Connection, GetUpdateQuery(obj), obj);
            if (result.Equals(0))
            {
                IEnumerable<dynamic> insertQueryResult = SqlMapper.Query(Connection, GetInsertQuery(obj), obj);
                long newRecordId = 0;
                if (insertQueryResult.FirstOrDefault() != null)
                {
                    newRecordId = insertQueryResult.FirstOrDefault().ID;
                }
                return newRecordId != 0;
            }
            return result == 1;
        }

        private string GetSoftDeleteQuery()
        {
            string tableName = GetTableNameT();
            return string.Format("UPDATE {0} SET Status = 0 WHERE ID=@ID", tableName);
        }

        private string GetTableNameT()
        {
            Type objtype = typeof(T);
            string tableName = "";
            object[] attrs = objtype.GetCustomAttributes(true);
            foreach (object attr in attrs)
            {
                DBTable authAttr = attr as DBTable;
                if (authAttr != null)
                    tableName = authAttr.Name;
            }

            return tableName;
        }

        private string GetSoftDeleteQueryWithModificationInfo()
        {
            string tableName = GetTableNameT();
            return string.Format("UPDATE {0} SET Status = 0, ModifiedOn = @ModifiedOn , ModifiedBy = @ModifiedBy WHERE ID=@ID", tableName);
        }
        private string GetSoftDeleteQueryWithModificationInfo(dynamic whereClause)
        {
            string tableName = GetTableNameT();

            string whereClauseString = GetWhereClauseByObject(whereClause);

            return string.Format("UPDATE {0} SET Status = 0, ModifiedOn = @ModifiedOn , ModifiedBy = @ModifiedBy WHERE {1}", tableName, whereClauseString);
        }

        private static string GetWhereClauseByObject(dynamic whereClause)
        {
            PropertyInfo[] propswhereClause = whereClause.GetType().GetProperties();
            string[] whereClauses = propswhereClause.Select(p => p.Name).ToArray();
            var whereClauseList = whereClauses.Select(name => name + "=@" + name).ToList();
            return string.Join(" and ", whereClauseList);
        }

        private string GetSoftDeleteQueryForColumn(object columnNames)
        {
            string tableName = GetTableNameT();
            string whereClause = GetWhereClauseByObject(columnNames);
            return string.Format("UPDATE {0} SET Status = 0 WHERE {1}", tableName, whereClause);
        }

        private string GetAllQuery()
        {
            string tableName = GetTableNameT();
            PropertyInfo[] props = typeof(T).GetProperties();
            string[] columns = props.Select(p => p.Name).ToArray();

            string queryText = string.Format("SELECT {1} From {0}", tableName, string.Join(",", columns));
            if (columns.Contains("Status", StringComparer.OrdinalIgnoreCase))
            {
                queryText += " WHERE Status = 1";
            }

            return queryText;
        }

        private string GetAllByColumnsQuery(dynamic whereClause)
        {
            string tableName = GetTableNameT();

            PropertyInfo[] props = typeof(T).GetProperties();
            string[] columns = props.Select(p => p.Name).ToArray();
            string columnString = string.Join(",", columns);

            string whereClauseString = GetWhereClauseByObject(whereClause);

            return string.Format("SELECT {1} From {0} WHERE {2}", tableName, columnString, whereClauseString);
        }

        private string GetByColumnsQuery(dynamic whereClause)
        {
            string tableName = GetTableNameT();

            PropertyInfo[] props = typeof(T).GetProperties();
            string[] columns = props.Select(p => p.Name).ToArray();

            PropertyInfo[] propswhereClause = whereClause.GetType().GetProperties();
            string[] whereClauses = propswhereClause.Select(p => p.Name).ToArray();
            var WhereClause = whereClauses.Select(name => name + "=@" + name).ToList();

            return string.Format("SELECT TOP 1 {1} From {0} WHERE {2}", tableName, string.Join(",", columns), string.Join(" and ", WhereClause));
        }

        private string GetByIdQuery()
        {
            TableInfo tableInfo = GetTableInfoT();
            PropertyInfo[] props = typeof(T).GetProperties();
            string[] columns = props.Select(p => p.Name).ToArray();
            string whereClause = GetWhereClauseByPKCSV(tableInfo.PrimaryKeyCSV);

            return string.Format("SELECT {1} From {0} WHERE {2}", tableInfo.TableName, string.Join(",", columns), whereClause);
        }

        private static string GetWhereClauseByPKCSV(string pkCSV)
        {
            var whereClauseList = pkCSV.Split(',').Select(name => name + "=@" + name).ToList();
            return string.Join(" and ", whereClauseList);
        }

        private TableInfo GetTableInfoT()
        {
            Type objtype = typeof(T);
            TableInfo tableInfo = new TableInfo();
            object[] attrs = objtype.GetCustomAttributes(true);
            foreach (object attr in attrs)
            {
                DBTable authAttr = attr as DBTable;
                if (authAttr != null)
                {
                    tableInfo.TableName = authAttr.Name;
                    if (string.IsNullOrEmpty(authAttr.PrimaryKeyColumnCSV))
                    {
                        tableInfo.PrimaryKeyCSV = "Id";
                    }
                    else
                    {
                        tableInfo.PrimaryKeyCSV = authAttr.PrimaryKeyColumnCSV;
                    }
                }
            }
            return tableInfo;
        }

        private string GetCountQuery()
        {
            string tableName = GetTableNameT();
            return string.Format("SELECT Count(*) From {0} WHERE Status = 1", tableName);
        }

        private string GetCountByColumnsQuery(dynamic whereClause)
        {
            string tableName = GetTableNameT();

            PropertyInfo[] props = typeof(T).GetProperties();
            string[] columns = props.Select(p => p.Name).ToArray();

            PropertyInfo[] propswhereClause = whereClause.GetType().GetProperties();
            string[] whereClauses = propswhereClause.Select(p => p.Name).ToArray();
            var WhereClause = whereClauses.Where(var => GetPropValue(whereClause, var) != null).Select(name => name + " like '%" + GetPropValue(whereClause, name) + "%'").ToList();

            string whereString = "WHERE Status = 1 ";
            if (WhereClause.Count > 0)
            {
                whereString = whereString + string.Format(" and {0}", string.Join(" and ", WhereClause));
            }

            return string.Format("SELECT count(*) From {0} {1}", tableName, whereString);
        }

        private static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }

        private string GetDistinctColumnQuery(string columnName)
        {
            TableInfo tableInfo = GetTableInfoT();
            return string.Format("SELECT DISTINCT {1} From {0}", tableInfo.TableName, columnName);
        }
        private string GetDistinctColumnQuery(string columns, dynamic whereClause)
        {
            string tableName = GetTableNameT();
            string whereClauseString = GetWhereClauseByObject(whereClause);
            return string.Format("SELECT DISTINCT {1} From {0} WHERE {2}", tableName, columns, whereClauseString);
        }

        private string GetHardDeleteQuery()
        {
            TableInfo tableInfo = GetTableInfoT();
            string whereClause = GetWhereClauseByPKCSV(tableInfo.PrimaryKeyCSV);
            return string.Format("DELETE FROM {0} WHERE {1}", tableInfo.TableName, whereClause);
        }
        private string GetHardDeleteQuery(object columnCriteria)
        {
            TableInfo tableInfo = GetTableInfoT();
            string whereClause = GetWhereClauseByObject(columnCriteria);
            return string.Format("DELETE FROM {0} WHERE {1}", tableInfo.TableName, whereClause);
        }

        private string GetInsertQuery(dynamic item)
        {
            TableInfo tableInfo = GetTableInfoT();
            PropertyInfo[] props = item.GetType().GetProperties();

            string[] tableColumns = props.Select(p => p.Name).ToArray();
            string[] insertQueryColumns = tableColumns.Where(s => s != "Id").ToArray();

            string[] primaryKeyColumns = tableInfo.PrimaryKeyCSV.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            string outputColumns = "inserted." + string.Join(",inserted.", primaryKeyColumns);

            string insertQueryTemplate = "INSERT INTO {0} ({1}) OUTPUT " + outputColumns + " AS ID VALUES (@{2})";

            return string.Format(insertQueryTemplate,
                                 tableInfo.TableName,
                                 string.Join(",", insertQueryColumns),
                                 string.Join(",@", insertQueryColumns));
        }

        private string GetIsExistsByIdQuery()
        {
            string tableName = GetTableNameT();

            return string.Format("SELECT 1 From {0} WHERE ID=@ID", tableName);
        }

        private string GetIsExistsByPKColumnsQuery(dynamic whereClause, long idNotEqualTo)
        {
            string tableName = GetTableNameT();

            PropertyInfo[] propswhereClause = whereClause.GetType().GetProperties();
            string[] whereClauses = propswhereClause.Select(p => p.Name).ToArray();
            var WhereClause = whereClauses.Select(name => name + "=@" + name).ToList();
            if (idNotEqualTo != 0)
            {
                WhereClause.Add("Id <> " + idNotEqualTo);
            }
            return string.Format("SELECT 1 From {0} WHERE {1}", tableName, string.Join(" and ", WhereClause));
        }

        private string GetRestoreQuery()
        {
            string tableName = GetTableNameT();
            return string.Format("UPDATE {0} SET Status = 1 WHERE ID=@ID", tableName);
        }

        private string GetRestoreModifiedQuery()
        {
            string tableName = GetTableNameT();
            return string.Format("UPDATE {0} SET Status = 1, ModifiedOn = '@ModifiedOn', ModifiedBy = @ModifiedBy WHERE ID=@ID", tableName);
        }

        private string GetUpdateQuery(dynamic item)
        {
            TableInfo tableInfo = GetTableInfoT();
            PropertyInfo[] props = item.GetType().GetProperties();
            string[] columns = props.Select(p => p.Name).Where(s => s != "Id").ToArray();

            var parameters = columns.Select(name => name + "=@" + name).ToList();
            string whereClause = GetWhereClauseByPKCSV(tableInfo.PrimaryKeyCSV);
            return string.Format("UPDATE {0} SET {1} WHERE {2}", tableInfo.TableName, string.Join(",", parameters), whereClause);
        }
    }
}
