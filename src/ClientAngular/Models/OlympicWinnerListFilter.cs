using ClientAngular.Common;
using System.Collections.Generic;

namespace ClientAngular.Models
{
    public class OlympicWinnerListFilter : PaginationModelBase
    {
        public string SearchKeyword { get; set; }
        public int? OlympicWinnerId { get; set; }
        public SortModel[] SortModel { get; set; }
        public Dictionary<string, FilterModel> FilterModel { get; set; }
        public IEnumerable<OlympicWinnerGridFilterListItem> OlympicWinnerGridFilterListItem { get; set; }
    }
}
