using FantasyStockTradingApp.Models;
using FluentNHibernate.Mapping;
using NHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FantasyStockTradingApp.Mappings
{
    public class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            Id(x => x.id).GeneratedBy.Increment();

            Map(x => x.email);
            Map(x => x.password);
            Map(x => x.first_name);
            Map(x => x.last_name);
            Table("user_account");
        }
    }
}
