using FantasyStockTradingApp.Core.Entities;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FantasyStockTradingApp.Mappings
{
    public class AccountMap : ClassMap<Account>
    {
        public AccountMap()
        {
            Id(x => x.Id).GeneratedBy.Increment();

            Map(x => x.UserId).Column("user_id");
            Map(x => x.Balance).Column("balance");
            Map(x => x.PortfolioBalance).Column("portfolio_balance");
            Table("account_balance");
        }
    }
}
