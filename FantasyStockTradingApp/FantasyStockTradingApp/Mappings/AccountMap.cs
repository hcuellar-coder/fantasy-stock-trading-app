using FantasyStockTradingApp.Models;
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

            Map(x => x.User_Id);
            Map(x => x.Balance);
            Map(x => x.Portfolio_Balance);
            Table("account_balance");
        }
    }
}
