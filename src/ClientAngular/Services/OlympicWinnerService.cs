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

        public async Task<OlympicWinnerListFilter> GetOlympicWinnerList(OlympicWinnerListFilter olympicWinnerListFilter)
        {
            try
            {
                var result = await _olympicWinnerRepository.GetOlympicWinnerList(olympicWinnerListFilter);
                var query = result.OlympicWinnerGridFilterListItem.AsQueryable();

                #region Get Filtered Condition Delegate
                Func<string, FilterModel, List<object>, string> getConditionFromModel =
                    (string colName, FilterModel model, List<object> values) =>
                    {
                        string modelResult = "";

                        switch (model.filterType)
                        {
                            case "text":
                                switch (model.type)
                                {
                                    case "equals":
                                        modelResult = $"{colName} = \"{model.filter}\"";
                                        break;
                                    case "notEqual":
                                        modelResult = $"{colName} = \"{model.filter}\"";
                                        break;
                                    case "contains":
                                        modelResult = $"{colName}.Contains(@{values.Count})";
                                        values.Add(model.filter);
                                        break;
                                    case "notContains":
                                        modelResult = $"!{colName}.Contains(@{values.Count})";
                                        values.Add(model.filter);
                                        break;
                                    case "startsWith":
                                        modelResult = $"{colName}.StartsWith(@{values.Count})";
                                        values.Add(model.filter);
                                        break;
                                    case "endsWith":
                                        modelResult = $"!{colName}.StartsWith(@{values.Count})";
                                        values.Add(model.filter);
                                        break;
                                }
                                break;
                            case "number":
                                switch (model.type)
                                {
                                    case "equals":
                                        modelResult = $"{colName} = {model.filter}";
                                        break;
                                    case "notEqual":
                                        modelResult = $"{colName} <> {model.filter}";
                                        break;
                                    case "lessThan":
                                        modelResult = $"{colName} < {model.filter}";
                                        break;
                                    case "lessThanOrEqual":
                                        modelResult = $"{colName} <= {model.filter}";
                                        break;
                                    case "greaterThan":
                                        modelResult = $"{colName} > {model.filter}";
                                        break;
                                    case "greaterThanOrEqual":
                                        modelResult = $"{colName} >= {model.filter}";
                                        break;
                                    case "inRange":
                                        modelResult = $"({colName} >= {model.filter} AND {colName} <= {model.filterTo})";
                                        break;
                                }
                                break;
                            case "date":
                                values.Add(model.dateFrom);

                                switch (model.type)
                                {
                                    case "equals":
                                        modelResult = $"{colName} = @{values.Count - 1}";
                                        break;
                                    case "notEqual":
                                        modelResult = $"{colName} <> @{values.Count - 1}";
                                        break;
                                    case "lessThan":
                                        modelResult = $"{colName} < @{values.Count - 1}";
                                        break;
                                    case "lessThanOrEqual":
                                        modelResult = $"{colName} <= @{values.Count - 1}";
                                        break;
                                    case "greaterThan":
                                        modelResult = $"{colName} > @{values.Count - 1}";
                                        break;
                                    case "greaterThanOrEqual":
                                        modelResult = $"{colName} >= @{values.Count - 1}";
                                        break;
                                    case "inRange":
                                        values.Add(model.dateTo);
                                        modelResult = $"({colName} >= @{values.Count - 2} AND {colName} <= @{values.Count - 1})";
                                        break;
                                }
                                break;
                        }
                        return modelResult;
                    };
                #endregion

                foreach (var f in olympicWinnerListFilter.FilterModel)
                {
                    string condition, tmp;
                    List<object> conditionValues = new List<object>();

                    if (!string.IsNullOrWhiteSpace(f.Value.logicOperator))
                    {
                        tmp = getConditionFromModel(f.Key, f.Value.condition1, conditionValues);
                        condition = tmp;

                        tmp = getConditionFromModel(f.Key, f.Value.condition2, conditionValues);
                        condition = $"{condition} {f.Value.logicOperator} {tmp}";
                    }
                    else
                    {
                        tmp = getConditionFromModel(f.Key, f.Value, conditionValues);
                        condition = tmp;
                    }
                    if (conditionValues.Count == 0) query = query.Where(condition);
                    else query = query.Where(condition, conditionValues.ToArray());
                }

                foreach (var s in olympicWinnerListFilter.SortModel)
                {
                    switch (s.Sort)
                    {
                        case "asc":
                            query = query.OrderBy(s.ColId);
                            break;
                        case "desc":
                            query = query.OrderBy($"{s.ColId} descending");
                            break;
                    };
                };

                if (olympicWinnerListFilter.SortModel.Count() == 0)
                {
                    query = query.OrderBy(x => x.Id);
                }

                result.OlympicWinnerGridFilterListItem = query.ToList();
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}