using ClientAngular.Common;
using System.Collections.Generic;

namespace ClientAngular.Models
{
    public class OlympicWinnerListFilter : PaginationModelBase
    {
        public string SelectQuery { get; set; }
        public string WhereQuery { get; set; }
        public string GroupQuery { get; set; }
        public string SortQuery { get; set; }
        public string LimitQuery { get; set; }
        public string[] GroupKeys { get; set; }
        public RowGroupCols[] RowGroupCols { get; set; }
        public RowGroupCols[] ValueCols { get; set; }
        public SortModel[] SortModel { get; set; }
        public Dictionary<string, FilterModel> FilterModel { get; set; }
        public IEnumerable<OlympicWinnerGridFilterListItem> OlympicWinnerGridFilterListItem { get; set; }
    }
}
