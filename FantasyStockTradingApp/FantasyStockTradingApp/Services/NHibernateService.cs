using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.Extensions.Configuration;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace FantasyStockTradingApp.Services
{                                           
    public interface INHibernateService
    {
        ISession OpenSession();
        void CloseSession();
    }
    public class NHibernateService : INHibernateService
    {
        public IConfiguration _configuration;
        private const string CurrentSessionKey = "nhibernate.current_session";
        private readonly ISessionFactory _sessionFactory;

        public NHibernateService()
        {
            _sessionFactory = FluentConfigure();
        }
        public ISession OpenSession()
        {
            return _sessionFactory.OpenSession();
        }
        public void CloseSession()
        {
            _sessionFactory.Close();
        }
        public void CloseSessionFactory()
        {
            if (_sessionFactory != null)
            {
                _sessionFactory.Close();
            }
        }

        public static ISessionFactory FluentConfigure()
        {
            return Fluently.Configure()
               .Database(PostgreSQLConfiguration.PostgreSQL82
               .ConnectionString(c => 
                    c.Host("localhost")
                    .Port(5432)
                    .Database("FantasyStockTradingApp")
                    .Username("postgres")
                    .Password("postgres")))
               
               .Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.GetExecutingAssembly()))
               .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(false, true))
               .BuildSessionFactory();
        }

    }
}
