using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FantasyStockTradingApp.Models;
using FluentNHibernate.Mapping;

namespace FantasyStockTradingApp.Mappings
{
    public class UserLoginMap : ClassMap<UserLogin>
    {
        public UserLoginMap()
        {
            Id(x => x.id).GeneratedBy.Increment();

            Map(x => x.email);
            Map(x => x.password);
            Map(x => x.user_account_id);
            Table("user_login");
        }
    }
}
