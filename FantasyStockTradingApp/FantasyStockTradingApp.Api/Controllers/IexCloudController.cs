using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using FantasyStockTradingApp.Models;
using FantasyStockTradingApp.Core.Services;
using FantasyStockTradingApp.Core.Exceptions;
using System.IO;

namespace FantasyStockTradingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IexCloudController : ControllerBase
    {
        private readonly IIexCloudService _iexCloudService;
        private readonly string _path;

        public IexCloudController(IIexCloudService iexCloudService)
        {
            _iexCloudService = iexCloudService;
            _path = Path.GetFullPath(ToString());
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
                throw new GetQouteException(_path, "GetQuote()");
            }
            
        }

        [HttpGet("get_mostactive")]
        public ActionResult<List<QuoteModel>> GetMostActive()
        {
            try 
            {
                var MostActive_Result = _iexCloudService.GetMostActive().Result;

                var MostActive = MostActive_Result.AsEnumerable().Select(quote => new QuoteModel()
                {
                    Symbol = quote.Symbol,
                    CompanyName = quote.CompanyName,
                    LatestPrice = quote.LatestPrice,
                    Change = quote.Change,
                    ChangePercent = quote.ChangePercent

                });

                return MostActive.ToList();
            } 
            catch 
            {
                throw new GetMostActiveException(_path, "GetMostActive()");
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
            } catch
            {
                throw new GetHistoryException(_path, "GetHistory()");
            }

            
        }
    }
}
