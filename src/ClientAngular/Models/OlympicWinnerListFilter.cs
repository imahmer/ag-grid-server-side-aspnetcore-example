using ClientAngular.Common;
using System.Collections.Generic;

namespace ClientAngular.Models
{
    public class OlympicWinnerListFilter : PaginationModelBase
    {
        public string SearchKeyword { get; set; }
        public int? OlympicWinnerId { get; set; }
        public IEnumerable<OlympicWinnerGridFilterListItem> OlympicWinnerGridFilterListItem { get; set; }
    }
}
