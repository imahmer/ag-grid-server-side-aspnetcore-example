using ClientAngular.Models;
using ClientAngular.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientAngular.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OlympicWinnerController : ControllerBase
    {
        private readonly IOlympicWinnerService _olympicWinnerService;

        public OlympicWinnerController(IOlympicWinnerService olympicWinnerService)
        {
            _olympicWinnerService = olympicWinnerService;
        }

        [Route("GetOlympicWinnerList")]
        [HttpPost]
        public async Task<OlympicWinnerListFilter> GetOlympicWinnerList(OlympicWinnerListFilter olympicWinnerListFilter)
        {
            return await _olympicWinnerService.GetOlympicWinnerList(olympicWinnerListFilter);
        }

        [Route("GetOlympicWinnerGroupedList")]
        [HttpPost]
        public async Task<IEnumerable<IGrouping<string, OlympicWinnerGridFilterListItem>>> GetOlympicWinnerGroupedList(OlympicWinnerListFilter olympicWinnerListFilter)
        {
            return await _olympicWinnerService.GetOlympicWinnerGroupedList(olympicWinnerListFilter);
        }
    }
}
