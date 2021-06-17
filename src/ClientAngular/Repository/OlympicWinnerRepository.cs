using ClientAngular.Interfaces;
using Dapper;
using ClientAngular.Models;
using System;
using System.Data;
using System.Threading.Tasks;
using ClientAngular.Configuration;
using System.Linq;

namespace ClientAngular.Repository
{
    public class OlympicWinnerRepository : BaseRepository<OlympicWinner>, IOlympicWinnerRepository
    {
        private readonly IDataConnection _dataConnection;
        private readonly IDBConfig _dBConfig;

        public OlympicWinnerRepository(IDataConnection dataConnection, IDBConfig dBConfig) : base(dBConfig.ConnectionString.Replace("DB_Placeholder", dBConfig.GetDBName()))
        {
            _dataConnection = dataConnection;
            _dBConfig = dBConfig;
        }

        //public async Task<OlympicWinnerListFilter> GetOlympicWinnerList(OlympicWinnerListFilter olympicWinnerListFilter)
        //{
        //    using (var connection = _dataConnection.Connect(_dBConfig.GetDBName(), _dBConfig.ConnectionString))
        //    {
        //        try
        //        {
        //            var param = new DynamicParameters();
        //            param.Add("@SearchKeyword", olympicWinnerListFilter.SQLQuery);
        //            param.Add("@OlympicWinnerId", olympicWinnerListFilter.OlympicWinnerId);
        //            param.Add("@Startindex", olympicWinnerListFilter.StartIndex);
        //            param.Add("@Pagesize", olympicWinnerListFilter.PageSize);
        //            param.Add("@Totalrecords", olympicWinnerListFilter.TotalRecords, direction: ParameterDirection.Output);

        //            var olympicWinnerGridFilterListItem = await connection.QueryAsync<OlympicWinnerGridFilterListItem>("Usp_OlympicWinnerGetGridFilterList", param: param, commandType: CommandType.StoredProcedure);
        //            _dataConnection.Disconnect();
        //            olympicWinnerListFilter.TotalRecords = param.Get<long>("@Totalrecords");
        //            olympicWinnerListFilter.OlympicWinnerGridFilterListItem = olympicWinnerGridFilterListItem;
        //            return olympicWinnerListFilter;
        //        }
        //        catch (Exception ex)
        //        {
        //            _dataConnection.Disconnect();
        //            throw;
        //        }
        //    }
        //}

        public async Task<OlympicWinnerListFilter> GetOlympicWinnerList(OlympicWinnerListFilter olympicWinnerListFilter)
        {
            using (var connection = _dataConnection.Connect(_dBConfig.GetDBName(), _dBConfig.ConnectionString))
            {
                try
                {
                    var param = new DynamicParameters();
                    param.Add("@SelectQuery", olympicWinnerListFilter.SelectQuery);
                    param.Add("@WhereQuery", olympicWinnerListFilter.WhereQuery);
                    param.Add("@SortQuery", olympicWinnerListFilter.SortQuery);
                    param.Add("@LimitQuery", olympicWinnerListFilter.LimitQuery);
                    param.Add("@Totalrecords", olympicWinnerListFilter.TotalRecords, direction: ParameterDirection.Output);
                    var olympicWinnerGridFilterListItem = await connection.QueryAsync<OlympicWinnerGridFilterListItem>("Usp_Filtered_Data", param: param, commandType: CommandType.StoredProcedure);
                    _dataConnection.Disconnect();
                    olympicWinnerListFilter.OlympicWinnerGridFilterListItem = olympicWinnerGridFilterListItem;
                    olympicWinnerListFilter.TotalRecords = param.Get<long>("@Totalrecords");
                    return olympicWinnerListFilter;
                }
                catch (Exception ex)
                {
                    _dataConnection.Disconnect();
                    throw;
                }
            }
        }
    }
}
