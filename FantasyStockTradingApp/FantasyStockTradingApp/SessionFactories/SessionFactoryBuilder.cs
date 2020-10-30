using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace FantasyStockTradingApp.SessionFactories
{
    public class SessionFactoryBuilder
    {
        public static ISessionFactory BuildSessionFactory(string connectionStringName, bool create = false, bool update = false)
        {
            return Fluently.Configure()
                .Database(PostgreSQLConfiguration.Standard
                .ConnectionString(ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString))
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<NHibernate.Cfg.Mappings>())
                .CurrentSessionContext("call")
                .ExposeConfiguration(cfg => BuildSchema(cfg, create, update))
                .BuildSessionFactory();
        }

        private static void BuildSchema(NHibernate.Cfg.Configuration config, bool create = false, bool update = false)
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
