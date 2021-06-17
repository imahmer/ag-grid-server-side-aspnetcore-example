using ClientAngular.Models;
using ClientAngular.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Threading.Tasks;

namespace ClientAngular.Services
{
    public class OlympicWinnerService : IOlympicWinnerService
    {
        private readonly IOlympicWinnerRepository _olympicWinnerRepository;

        public OlympicWinnerService(IOlympicWinnerRepository olympicWinnerRepository)
        {
            _olympicWinnerRepository = olympicWinnerRepository;
        }

        public Task<IEnumerable<IGrouping<string, OlympicWinnerGridFilterListItem>>> GetOlympicWinnerGroupedList(OlympicWinnerListFilter olympicWinnerListFilter)
        {
            throw new NotImplementedException();
        }

        public async Task<OlympicWinnerListFilter> GetOlympicWinnerList(OlympicWinnerListFilter olympicWinnerListFilter)
        {
            try
            {
                string buildSQL = "{0} {1} {2} {3}";
                string selectQuery = "";
                string whereQuery = "";
                string sortQuery = "";

                #region Get Filtered Condition Delegate
                Func<string, FilterModel, string> getConditionFromModel =
                    (string colName, FilterModel model) =>
                    {
                        string modelResult = "";

                        switch (model.filterType)
                        {
                            case "text":
                                switch (model.type)
                                {
                                    case "equals":
                                        modelResult = $"{colName} = '{model.filter}'";
                                        break;
                                    case "notEqual":
                                        modelResult = $"{colName} <> '{model.filter}'";
                                        break;
                                    case "contains":
                                        modelResult = $"{colName} LIKE '%{model.filter}%'";
                                        break;
                                    case "notContains":
                                        modelResult = $"{colName} NOT LIKE '%{model.filter}%'";
                                        break;
                                    case "startsWith":
                                        modelResult = $"{colName} LIKE '{model.filter}%'";
                                        break;
                                    case "endsWith":
                                        modelResult = $"{colName} LIKE '%{model.filter}'";
                                        break;
                                }
                                break;
                        }
                        return modelResult;
                    };
                #endregion

                #region Get Limit Condition Delegate
                Func<int, int, string> setLimitSQL =
                    (int startPage, int pageSize) =>
                    {
                        startPage = startPage - 1;
                        return $"OFFSET {startPage}  ROWS FETCH NEXT {pageSize} ROWS ONLY OPTION (RECOMPILE)";
                    };
                #endregion

                selectQuery = $"select {String.Join(",", typeof(OlympicWinnerGridFilterListItem).GetProperties().Select(x => x.Name).ToArray())} from OlympicWinners ";

                if (olympicWinnerListFilter.FilterModel.Count() > 0)
                    whereQuery += "WHERE 1=1 ";

                foreach (var f in olympicWinnerListFilter.FilterModel)
                {
                    string condition, tmp;

                    if (!string.IsNullOrWhiteSpace(f.Value.logicOperator))
                    {
                        tmp = getConditionFromModel(f.Key, f.Value.condition1);
                        condition = tmp;

                        tmp = getConditionFromModel(f.Key, f.Value.condition2);
                        condition = $"AND ({condition} {f.Value.logicOperator} {tmp})";
                    }
                    else
                    {
                        tmp = getConditionFromModel(f.Key, f.Value);
                        condition = $"AND {tmp}";
                    }
                    whereQuery += condition;
                }
                if (olympicWinnerListFilter.SortModel.Length > 0)
                    sortQuery += "ORDER BY ";
                else if(olympicWinnerListFilter.SortModel.Length == 0)
                    sortQuery += $"ORDER BY {typeof(OlympicWinnerGridFilterListItem).GetProperties().Select(x => x.Name).FirstOrDefault()}";


                foreach (var s in olympicWinnerListFilter.SortModel)
                {
                    switch (s.Sort)
                    {
                        case "asc":
                            sortQuery += $"{s.ColId}";
                            break;
                        case "desc":
                            sortQuery += $"{s.ColId} desc";
                            break;
                    };
                };

                //buildSQL = string.Format(buildSQL, selectQuery, whereQuery, sortQuery, setLimitSQL(olympicWinnerListFilter.StartIndex, olympicWinnerListFilter.PageSize));
                olympicWinnerListFilter.SelectQuery = selectQuery;
                olympicWinnerListFilter.WhereQuery = whereQuery;
                olympicWinnerListFilter.SortQuery = sortQuery;
                olympicWinnerListFilter.LimitQuery = setLimitSQL(olympicWinnerListFilter.StartIndex, olympicWinnerListFilter.PageSize);
                var result = await _olympicWinnerRepository.GetOlympicWinnerList(olympicWinnerListFilter);
                
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}