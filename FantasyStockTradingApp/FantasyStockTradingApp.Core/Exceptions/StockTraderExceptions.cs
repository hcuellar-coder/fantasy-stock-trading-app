using System;

namespace FantasyStockTradingApp.Core.Exceptions
{
    public abstract class StockTraderExceptions : Exception
    {
        public string Path { get; }
        public string Method { get; }

        internal StockTraderExceptions(string path, string method, string message) : base(message)
        {
            Path = path;
            Method = method;
        }
    }
}
