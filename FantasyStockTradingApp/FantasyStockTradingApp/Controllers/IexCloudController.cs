using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FantasyStockTradingApp.Models;
using FantasyStockTradingApp.Core.Services;

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
        public IQueryable<QuoteModel> GetQuote(string symbol)
        {
            Console.WriteLine("in get_quote");
            Console.WriteLine("symbol = " + symbol);

            return (IQueryable<QuoteModel>)_iexCloudService.GetQuote(symbol);
        }

        [HttpGet("get_mostactive")]
        public IQueryable<List<QuoteModel>> GetMostActive()
        {
            Console.WriteLine("in get_mostactive");
            //var MostActive = await _iexCloudService.GetMostActive();
            return (IQueryable<List<QuoteModel>>)_iexCloudService.GetMostActive();
            /*return (Task<List<QuoteModel>>)_iexCloudService.GetMostActive().Result.Select(quote => new QuoteModel {
                Symbol = quote.Symbol,
                CompanyName = quote.CompanyName,
                LatestPrice = quote.LatestPrice,
                Change = quote.Change,
                ChangePercent = quote.ChangePercent
            });*/
        }

        /*
         public string Symbol { get; set; }
        public string CompanyName { get; set; }
        public float LatestPrice { get; set; }
        public float Change { get; set; }
        public float ChangePercent { get; set; } 
         * */


        [HttpGet("get_history")]
        public IQueryable<List<History>> GetHistory(string symbol)
        {
            Console.WriteLine("in get_quote");
            Console.WriteLine("symbol = " + symbol);

            return (IQueryable<List<History>>)_iexCloudService.GetHistory(symbol);
        }
    }
}
