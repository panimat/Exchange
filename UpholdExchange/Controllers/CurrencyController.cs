using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DBRepository.Interfaces;
using Models;

namespace UpholdExchange.Controllers
{
    [Route("api/currencies")]
    public class CurrencyController : Controller
    {
        ICurrencyRepository _currencyRepository;

        public CurrencyController(ICurrencyRepository currencyRepository)
        {
            _currencyRepository = currencyRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<Currencies>> Get()
        {
            return await _currencyRepository.GetCurrencies();
        }

        public IActionResult Index()
        {
            return Ok();
        }
    }
}