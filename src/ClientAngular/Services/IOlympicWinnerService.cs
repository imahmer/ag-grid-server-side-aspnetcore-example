using ClientAngular.Models;
using System.Threading.Tasks;

namespace ClientAngular.Services
{
    public interface IOlympicWinnerService
    {
        Task<OlympicWinnerListFilter> GetOlympicWinnerList(OlympicWinnerListFilter olympicWinnerListFilter);
    }
}
