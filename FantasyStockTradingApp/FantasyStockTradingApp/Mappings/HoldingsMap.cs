using FantasyStockTradingApp.Models;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FantasyStockTradingApp.Mappings
{
    public class HoldingsMap : ClassMap<HoldingsModel>
    {
        public HoldingsMap()
        {
            Id(x => x.Id).GeneratedBy.Increment();

            Map(x => x.Account_Id);
            Map(x => x.Company_Name);
            Map(x => x.Symbol);
            Map(x => x.Stock_Count);
            Map(x => x.Latest_Cost_per_Stock);
            Map(x => x.Change);
            Map(x => x.Change_Percentage);
            Map(x => x.Last_Updated);
            Table("holdings");
        }
    }
}
