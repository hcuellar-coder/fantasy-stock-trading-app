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
        public ActionResult<QuoteModel> GetQuote(string symbol)
        {
                var Quote_Result = _iexCloudService.GetQuote(symbol).Result;

                var Quote_Return = new QuoteModel()
                {
                    Symbol = Quote_Result.Symbol,
                    CompanyName = Quote_Result.CompanyName,
                    LatestPrice = Quote_Result.LatestPrice,
                    Change = Quote_Result.Change,
                    ChangePercent = Quote_Result.ChangePercent
                };

                return Quote_Return;
        }

        [HttpGet("get_mostactive")]
        public ActionResult<List<QuoteModel>> GetMostActive()
        {
            var Quote_Result = _iexCloudService.GetMostActive().Result;

            var MostActive = Quote_Result.AsEnumerable().Select(quote => new QuoteModel()
            {
                Symbol = quote.Symbol,
                CompanyName = quote.CompanyName,
                LatestPrice = quote.LatestPrice,
                Change = quote.Change,
                ChangePercent = quote.ChangePercent

            });

            return MostActive.ToList();
        }

        [HttpGet("get_history")]
        public ActionResult<List<History>> GetHistory(string symbol)
        {

            var History_Result = _iexCloudService.GetHistory(symbol).Result;

            var History = History_Result.AsEnumerable().Select(history => new History()
            {
                Date = history.Date,
                Close = history.Close,
            });

            return History.ToList();
        }
    }
}
