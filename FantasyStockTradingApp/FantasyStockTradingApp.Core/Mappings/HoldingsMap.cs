using FantasyStockTradingApp.Core.Entities;
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

            Map(x => x.AccountId).Column("account_id");
            Map(x => x.CompanyName).Column("company_name");
            Map(x => x.Symbol).Column("symbol");
            Map(x => x.StockCount).Column("stock_count");
            Map(x => x.LatestCostPerStock).Column("latest_cost_per_stock");
            Map(x => x.Change).Column("change");
            Map(x => x.ChangePercentage).Column("change_percentage");
            Map(x => x.LastUpdated).Column("last_updated");
            Table("holdings");
        }
    }
}
