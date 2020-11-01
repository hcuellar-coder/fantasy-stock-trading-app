using FantasyStockTradingApp.Mappings;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.Extensions.Configuration;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
//using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace FantasyStockTradingApp.SessionFactories
{
    public class SessionFactoryBuilder
    {
            public SessionFactoryBuilder(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public ISessionFactory BuildSessionFactory(string connectionStringName, bool create = false, bool update = false)
        {
            return Fluently.Configure()
               .Database(PostgreSQLConfiguration.Standard.ConnectionString(Configuration.GetConnectionString("DefaultConnection")))
               .Mappings(m => m.FluentMappings.AddFromAssembly(GetType().Assembly))
               .CurrentSessionContext("call")
               .ExposeConfiguration(cfg => BuildSchema(cfg, create, update))
               .BuildSessionFactory();
        }

        private static void BuildSchema(Configuration config, bool create = false, bool update = false)
        {
            if (create)
            {
                new SchemaExport(config).Create(false, true);
            } else
            {
                new SchemaUpdate(config).Execute(false, update);
            }
        }
    }
}



/*return Fluently.Configure()
                .Database(PostgreSQLConfiguration.Standard
                .ConnectionString(ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString))
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<UserMap>())
                .CurrentSessionContext("call")
                .ExposeConfiguration(cfg => BuildSchema(cfg, create, update))
                .BuildSessionFactory();*/