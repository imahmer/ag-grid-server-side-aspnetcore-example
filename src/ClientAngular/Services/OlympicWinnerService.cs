using ClientAngular.Models;
using ClientAngular.Repository;
using System;
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
                return await _olympicWinnerRepository.GetOlympicWinnerList(olympicWinnerListFilter);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}