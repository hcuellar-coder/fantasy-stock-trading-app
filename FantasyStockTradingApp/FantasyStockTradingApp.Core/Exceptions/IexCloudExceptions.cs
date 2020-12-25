using System;
using System.Collections.Generic;
using System.Text;

namespace FantasyStockTradingApp.Core.Exceptions
{
    class IexCloudExceptions
    {
    }

    public class GetQouteException : StockTraderExceptions
    {
        public GetQouteException(string path, string method)
            : base(path, method, "Error getting quote")
        {

        }
    }

    public class GetMostActiveException : StockTraderExceptions
    {
        public GetMostActiveException(string path, string method)
            : base(path, method, "Error getting most active stocks")
        {

        }
    }

    public class GetHistoryException : StockTraderExceptions
    {
        public GetHistoryException(string path, string method)
            : base(path, method, "Error getting history")
        {

        }
    }
}
