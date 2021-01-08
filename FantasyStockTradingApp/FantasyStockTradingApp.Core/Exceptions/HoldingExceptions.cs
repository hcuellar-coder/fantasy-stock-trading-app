namespace FantasyStockTradingApp.Core.Exceptions
{
    public class HoldingExistsException : StockTraderExceptions
    {
        public HoldingExistsException(string path, string method)
            : base(path, method, "Holding does not exist")
        {

        }
    }

    public class GetHoldingException : StockTraderExceptions
    {
        public GetHoldingException(string path, string method)
            : base(path, method, "Error getting holding")
        {

        }
    }

    public class NewHoldingException : StockTraderExceptions
    {
        public NewHoldingException(string path, string method)
            : base(path, method, "Error creating holding")
        {

        }
    }

    public class UpdateHoldingException : StockTraderExceptions
    {
        public UpdateHoldingException(string path, string method)
            : base(path, method, "Error updating single holding")
        {

        }
    }

    public class UpdateHoldingsException : StockTraderExceptions
    {
        public UpdateHoldingsException(string path, string method)
            : base(path, method, "Error updating user holdings")
        {

        }
    }

    public class DeleteHoldingsException : StockTraderExceptions
    {
        public DeleteHoldingsException(string path, string method)
            : base(path, method, "Error deleting user holding")
        {

        }
    }

}
