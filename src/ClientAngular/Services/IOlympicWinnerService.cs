using ClientAngular.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientAngular.Services
{
    public interface IOlympicWinnerService
    {
        Task<OlympicWinnerListFilter> GetOlympicWinnerList(OlympicWinnerListFilter olympicWinnerListFilter);
        Task<IEnumerable<IGrouping<string, OlympicWinnerGridFilterListItem>>> GetOlympicWinnerGroupedList(OlympicWinnerListFilter olympicWinnerListFilter);
    }
}
