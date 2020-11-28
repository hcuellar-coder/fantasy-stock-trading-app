using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FantasyStockTradingApp.Core.Entities
{
    public class Transaction
    {
        public virtual int Id { get; set; }
        public virtual int Account_Id { get; set; }
        public virtual string Type { get; set; }
        public virtual string Symbol { get; set; }
        public virtual int Stock_Count { get; set; }
        public virtual float Cost_per_Stock { get; set; }
        public virtual float Cost_per_Transaction { get; set; }
        public virtual DateTime Transaction_Date { get; set; }

    }
}
