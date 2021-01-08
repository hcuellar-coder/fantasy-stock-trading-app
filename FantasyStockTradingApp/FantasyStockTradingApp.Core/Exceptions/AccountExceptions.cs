namespace FantasyStockTradingApp.Core.Exceptions
{
    public class GetAccountException : StockTraderExceptions
    {
        public GetAccountException(string path, string method)
            : base(path, method, "Error getting account")
        {

        }
    }

    public class NewAccountException : StockTraderExceptions
    {
        public NewAccountException(string path, string method)
            : base(path, method, "Error creating account")
        {

        }
    }

    public class UpdateAccountException : StockTraderExceptions
    {
        public UpdateAccountException(string path, string method)
            : base(path, method, "Error updating account")
        {

        }
    }

}
