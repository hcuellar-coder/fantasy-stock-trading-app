using FantasyStockTradingApp.Core.Entities;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FantasyStockTradingApp.Mappings
{
    public class TransactionMap : ClassMap<Transaction>
    {
        public TransactionMap()
        {
            Id(x => x.Id).GeneratedBy.Increment();

            Map(x => x.AccountId).Column("account_id");
            Map(x => x.Type).Column("type");
            Map(x => x.Symbol).Column("symbol");
            Map(x => x.StockCount).Column("stock_count");
            Map(x => x.CostPerStock).Column("cost_per_stock");
            Map(x => x.CostPerTransaction).Column("cost_per_transaction");
            Map(x => x.TransactionDate).Column("transaction_date");

            Table("transactions");
        }
    }
}
