using FantasyStockTradingApp.Core.Entities;
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
            Id(x => x.Id).GeneratedBy.Increment();

            Map(x => x.Email).Column("email");
            Map(x => x.Password).Column("password");
            Map(x => x.FirstName).Column("first_name");
            Map(x => x.LastName).Column("last_name");
            Table("user_account");
        }
    }
}
