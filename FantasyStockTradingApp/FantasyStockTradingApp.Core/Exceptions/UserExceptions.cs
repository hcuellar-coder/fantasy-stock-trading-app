namespace FantasyStockTradingApp.Core.Exceptions
{
    public class IsUserValidException : StockTraderExceptions
    {
        public IsUserValidException(string path, string method)
            : base(path, method, "User does not exist")
        {

        }
    }
    public class GetUserException : StockTraderExceptions
    { 
        public GetUserException(string path, string method)
            : base(path, method, "User Not Found")
        {

        }
    }
    public class NewUserInsertException : StockTraderExceptions
    {
        public NewUserInsertException(string path, string method)
            : base(path, method, "Error Inserting New User")
        {

        }
    }

    public class NewUserEmailException : StockTraderExceptions
    {
        public NewUserEmailException(string path, string method)
            : base(path, method, "Incorrect Email Format")
        {

        }
    }

}
