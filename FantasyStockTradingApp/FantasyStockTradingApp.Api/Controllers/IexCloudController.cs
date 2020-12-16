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
            try
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

            } catch
            {
                return StatusCode(500, "There was an error getting quote data");
            }
            
            
        }

        [HttpGet("get_mostactive")]
        public ActionResult<List<QuoteModel>> GetMostActive()
        {
            try
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
            } catch
            {
                return StatusCode(500, "There was an error getting most active");
            }
           
        }

        [HttpGet("get_history")]
        public ActionResult<List<History>> GetHistory(string symbol)
        {
            try
            {
                var History_Result = _iexCloudService.GetHistory(symbol).Result;

                var History = History_Result.AsEnumerable().Select(history => new History()
                {
                    Date = history.Date,
                    Close = history.Close,
                });

                return History.ToList();
            }
            catch
            {
                return StatusCode(500, "There was an error getting history");
            }
        }
    }
}
