using FantasyStockTradingApp.Models;
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

            Map(x => x.Cost);
            Map(x => x.Stock_count);
            Map(x => x.Symbol);
            Map(x => x.Type);
            Table("transactions");
        }
    }
}
