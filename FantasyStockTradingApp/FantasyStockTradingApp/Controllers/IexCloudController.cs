using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FantasyStockTradingApp.Models;
using FantasyStockTradingApp.Services;

namespace FantasyStockTradingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IexCloudController : ControllerBase
    {
        private readonly IIexCloudService _iexCloudService;

        public IexCloudController(IIexCloudService iexCloudService)
        {
            _iexCloudService = iexCloudService;
        }

        [HttpGet("get_quote")]
        public Task<Quote> GetQuote(string symbol)
        {
            Console.WriteLine("in get_quote");
            Console.WriteLine("symbol = " + symbol);

            return _iexCloudService.GetQuote(symbol);
        }

        [HttpGet("get_mostactive")]
        public Task<List<Quote>> GetMostActive()
        {
            Console.WriteLine("in get_mostactive");

            return _iexCloudService.GetMostActive();
        }

        [HttpGet("get_history")]
        public Task<List<History>> GetHistory(string symbol)
        {
            Console.WriteLine("in get_quote");
            Console.WriteLine("symbol = " + symbol);

            return _iexCloudService.GetHistory(symbol);
        }
    }
}
