using FantasyStockTradingApp.Models;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FantasyStockTradingApp.Mappings
{
    public class HoldingsMap : ClassMap<Holdings>
    {
        public HoldingsMap()
        {
            Id(x => x.Id).GeneratedBy.Increment();
            Map(x => x.Account_Id);
            Map(x => x.Symbol);
            Map(x => x.Stock_Count);
            Map(x => x.Latest_Price);
            Map(x => x.Last_Updated);
            Table("holdings");
        }
    }
}
