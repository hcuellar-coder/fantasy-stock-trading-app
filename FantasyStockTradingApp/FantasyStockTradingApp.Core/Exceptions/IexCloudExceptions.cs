namespace FantasyStockTradingApp.Core.Exceptions
{
    public class GetQouteException : StockTraderExceptions
    {
        public GetQouteException(string path, string method)
            : base(path, method, "Symbol could not be found, try a different symbol!")
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
