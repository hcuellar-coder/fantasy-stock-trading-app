using System;
using System.Collections.Generic;
using System.Text;

namespace FantasyStockTradingApp.Core.Exceptions
{
    class TransactionExceptions
    {
    }

    public class NewTransactionException : StockTraderExceptions
    {
        public NewTransactionException(string path, string method)
            : base(path, method, "Error creating transaction")
        {

        }
    }
}
