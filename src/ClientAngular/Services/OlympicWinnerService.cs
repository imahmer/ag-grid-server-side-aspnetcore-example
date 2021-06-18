using ClientAngular.Models;
using ClientAngular.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
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
                        return $"OFFSET {startPage} ROWS FETCH NEXT {pageSize} ROWS ONLY OPTION (RECOMPILE)";
                    };
                #endregion

                string selectQuery = selectSQL(olympicWinnerListFilter);

                string whereQuery = whereSQL(olympicWinnerListFilter);

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

                string sortQuery = "ORDER BY";
                if (olympicWinnerListFilter.SortModel.Length == 0)
                {
                    if (isDoingGrouping(olympicWinnerListFilter.RowGroupCols, olympicWinnerListFilter.GroupKeys))
                        sortQuery += $" {olympicWinnerListFilter.RowGroupCols[olympicWinnerListFilter.GroupKeys.Length].Id}";
                    else
                        sortQuery += $" {typeof(OlympicWinnerGridFilterListItem).GetProperties().Select(x => x.Name).FirstOrDefault()}";
                }

                foreach (var s in olympicWinnerListFilter.SortModel)
                {
                    switch (s.Sort)
                    {
                        case "asc":
                            sortQuery += $" {s.ColId}";
                            break;
                        case "desc":
                            sortQuery += $" {s.ColId} desc";
                            break;
                    };
                };

                olympicWinnerListFilter.SelectQuery = selectQuery;
                olympicWinnerListFilter.WhereQuery = whereQuery;
                olympicWinnerListFilter.GroupQuery = groupBySql(olympicWinnerListFilter);
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

        private string selectSQL(OlympicWinnerListFilter olympicWinnerListFilter)
        {
            var rowGroupCols = olympicWinnerListFilter.RowGroupCols;
            var valueCols = olympicWinnerListFilter.ValueCols;
            var groupKeys = olympicWinnerListFilter.GroupKeys;
            var colsToSelect = new List<string>();
            var tableName = "OlympicWinners";

            if (isDoingGrouping(rowGroupCols, groupKeys))
            {
                colsToSelect.Add(rowGroupCols[groupKeys.Length].Id);

                foreach (var valueCol in valueCols)
                {
                    colsToSelect.Add($"{valueCol.aggFunc}({valueCol.Id}) AS {valueCol.Id}");
                }

                return $"SELECT {string.Join(", ", colsToSelect.ToArray())} from {tableName}";
            }

            return $"select * from {tableName}";
        }

        private string whereSQL(OlympicWinnerListFilter olympicWinnerListFilter)
        {
            var rowGroups = olympicWinnerListFilter.RowGroupCols;
            var groupKeys = olympicWinnerListFilter.GroupKeys;
            var whereParts = new List<string>();

            if (groupKeys.Length > 0)
            {
                foreach (var item in groupKeys.Select((value, i) => new { i, value }))
                {
                    var value = item.value;
                    var index = item.i;
                    whereParts.Add($"{rowGroups[index].Id} = '{value}'");
                }
                return $"WHERE {string.Join(" AND ", whereParts.ToArray())}";
            }

            return "WHERE 1=1 ";
        }

        private string groupBySql(OlympicWinnerListFilter olympicWinnerListFilter)
        {
            var rowGroupCols = olympicWinnerListFilter.RowGroupCols;
            var groupKeys = olympicWinnerListFilter.GroupKeys;

            if (isDoingGrouping(rowGroupCols, groupKeys))
            {
                return $"GROUP BY {rowGroupCols[groupKeys.Length].Id}";
            }

            return "";
        }

        private bool isDoingGrouping(RowGroupCols[] rowGroupCols, string[] groupKeys)
        {
            return rowGroupCols.Length > groupKeys.Length;
        }

        private int getLastRowIndex(int StartIndex, int PageSize, OlympicWinnerListFilter results)
        {
            if (results.OlympicWinnerGridFilterListItem == null || results.TotalRecords == 0)
                return -1;

            var currentLastRow = StartIndex + results.OlympicWinnerGridFilterListItem.Count();

            return currentLastRow <= PageSize ? currentLastRow : -1;
        }
    }
}