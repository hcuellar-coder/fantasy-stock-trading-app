﻿using System;

namespace FantasyStockTradingApp.Core.Entities
{
    public class Transaction
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
