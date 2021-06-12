using System.Collections.Generic;
using System.Data.Common;

namespace ClientAngular.Interfaces
{
    public interface IBaseRepository<T>
    {
        long Insert(T obj);
        long Insert(T obj, DbTransaction dbTransaction);
        bool Update(T obj);
        bool Upsert(T obj);
        bool HardDelete(T obj);
        bool HardDelete(object columnCriteria);
        bool Delete(long id);
        bool DeleteByColumnName(object columns);
        bool DeleteModified(T obj);
        bool DeleteByColumnCriteria(T obj, object columns);
        bool Restore(long id);
        bool RestoreModified(T obj);
        IEnumerable<T> GetAll();
        IEnumerable<T> GetAllByColumns(object columns);
        long GetCount();
        long GetCountByColumns(object columns);
        T GetById(long id);
        T GetByColumns(object columns);
        bool IsExists(long id);
        bool IsExists(object whereColumns, long idNotEqualTo = 0);
        IEnumerable<T> GetDistinctColumnByName(string columnName);
        IEnumerable<T> GetDistinctColumnByName(string columns, object whereClause);
    }
}