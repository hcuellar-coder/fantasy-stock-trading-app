using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FantasyStockTradingApp.Core.Exceptions;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace FantasyStockTradingApp.Api.Models
{
    public class StockTraderExceptionModel
    {
        public StockTraderExceptionModel(StockTraderExceptions ex)
        {
            Type = ex.GetType().ToString();
            Path = $"{ex.Path}.{ ex.Method }";
            Message = $"{ex.Message}";
        }

        public string Type { get; set; }
        public string Path { get; set; }
        public string Message { get; set; }

        public override string ToString() => JsonSerializer.Serialize(this);
    }
}
