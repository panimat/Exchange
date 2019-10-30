using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Models;
using DBRepository.Interfaces;

namespace UpholdExchange.Controllers
{
    [Route("api/Rates")]
    public class RateController : Controller
    {
        IRateRepository _rateRepository;

        public RateController(IRateRepository rateRepository)
        {
            _rateRepository = rateRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<Rate>> Get()
        {
            return await _rateRepository.GetRates();
        }
        
        [HttpGet("{pair}/{sum}")]
        public double Get(string pair, double sum)
        {
            return Math.Round(_rateRepository.GetAsk(pair) * sum, 3);
        }

        [HttpPost]
        public IActionResult Post(string pair)
        {
            return Ok(_rateRepository.GetAsk(pair));
        }
    }
}