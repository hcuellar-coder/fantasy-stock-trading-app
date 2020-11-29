using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FantasyStockTradingApp.Models
{
    public class TransactionModel
    {
        public virtual int Id { get; set; }
        public virtual int AccountId { get; set; }
        public virtual string Type { get; set; }
        public virtual string Symbol { get; set; }
        public virtual int StockCount { get; set; }
        public virtual float CostPerStock { get; set; }
        public virtual float CostPerTransaction { get; set; }
        public virtual DateTime TransactionDate { get; set; }

    }
}
