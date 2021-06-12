using ClientAngular.Interfaces;
using ClientAngular.Models;
using System.Threading.Tasks;

namespace ClientAngular.Repository
{
    public interface IOlympicWinnerRepository : IBaseRepository<OlympicWinner>
    {
        Task<OlympicWinnerListFilter> GetOlympicWinnerList(OlympicWinnerListFilter olympicWinnerListFilter);
    }
}
