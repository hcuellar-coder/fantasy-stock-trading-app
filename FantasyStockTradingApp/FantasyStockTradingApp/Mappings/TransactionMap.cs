using FantasyStockTradingApp.Models;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FantasyStockTradingApp.Mappings
{
    public class TransactionMap : ClassMap<TransactionModel>
    {
        public TransactionMap()
        {
            Id(x => x.Id).GeneratedBy.Increment();

            Map(x => x.Account_Id);
            Map(x => x.Type);
            Map(x => x.Symbol);
            Map(x => x.Stock_Count);
            Map(x => x.Cost_per_Stock);
            Map(x => x.Cost_per_Transaction);
            Map(x => x.Transaction_Date);

            Table("transactions");
        }
    }
}
