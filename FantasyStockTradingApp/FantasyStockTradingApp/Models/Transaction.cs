using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FantasyStockTradingApp.Models
{
    public class Transaction
    {
        public virtual long Id { get; set; }
        public virtual long Account_Id { get; set; }
        public virtual string Type { get; set; }
        public virtual string Symbol { get; set; }
        public virtual int Stock_count { get; set; }
        public virtual float Cost { get; set; }
        public virtual DateTime Date_of_Transaction { get; set; }

    }
}
