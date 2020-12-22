using System;
using System.Collections.Generic;
using System.Text;

namespace FantasyStockTradingApp.Core.Exceptions
{
    public class UserException : StockTraderExceptions
    { 
        public UserException(string path, string method)
            : base(path, method, "New User Exception Error")
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
