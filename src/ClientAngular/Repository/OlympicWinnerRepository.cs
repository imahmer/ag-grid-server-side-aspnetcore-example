using ClientAngular.Interfaces;
using Dapper;
using ClientAngular.Models;
using System;
using System.Data;
using System.Threading.Tasks;
using ClientAngular.Configuration;

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

        public async Task<OlympicWinnerListFilter> GetOlympicWinnerList(OlympicWinnerListFilter olympicWinnerListFilter)
        {
            using (var connection = _dataConnection.Connect(_dBConfig.GetDBName(), _dBConfig.ConnectionString))
            {
                try
                {
                    var param = new DynamicParameters();

                    param.Add("@SelectQuery", olympicWinnerListFilter.SelectQuery);
                    param.Add("@WhereQuery", olympicWinnerListFilter.WhereQuery);
                    param.Add("@GroupQuery", olympicWinnerListFilter.GroupQuery);
                    param.Add("@SortQuery", olympicWinnerListFilter.SortQuery);
                    param.Add("@LimitQuery", olympicWinnerListFilter.LimitQuery);
                    param.Add("@TotalRecords", olympicWinnerListFilter.TotalRecords, direction: ParameterDirection.Output);

                    var olympicWinnerGridFilterListItem = await connection.QueryAsync<OlympicWinnerGridFilterListItem>("Usp_Filtered_Data", param: param, commandType: CommandType.StoredProcedure);
                    _dataConnection.Disconnect();
                    olympicWinnerListFilter.TotalRecords = param.Get<long>("@TotalRecords");
                    olympicWinnerListFilter.OlympicWinnerGridFilterListItem = olympicWinnerGridFilterListItem;
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
