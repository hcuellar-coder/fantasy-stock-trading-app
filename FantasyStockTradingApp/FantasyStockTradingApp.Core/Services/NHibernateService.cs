using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace FantasyStockTradingApp.Core.Services
{                                           
    public interface INHibernateService
    {
        ISession OpenSession();
        void CloseSession();
    }
    public class NHibernateService : INHibernateService
    {
        private readonly ISessionFactory _sessionFactory;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public NHibernateService(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
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
        public ISessionFactory FluentConfigure()
        {
            if (_hostingEnvironment.EnvironmentName == "Development")
            {
                Console.WriteLine("In Development NHibernate");
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
            } else
            {
               var DBPASSWORD = Environment.GetEnvironmentVariable("DBPASSWORD");
                Console.WriteLine("In Producation NHibernate");
                return Fluently.Configure()
               .Database(PostgreSQLConfiguration.PostgreSQL82
               .ConnectionString(c =>
                    c.Host("fantasystocktradingapp-hcuellar-postgresql.postgres.database.azure.com")
                    .Port(5432)
                    .Database("stocktradingapp")
                    .Username("postgreSQLAdmin@fantasystocktradingapp-hcuellar-postgresql")
                    .Password(DBPASSWORD)
                    )
               )
               .Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.GetExecutingAssembly()))
               .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(false, true))
               .BuildSessionFactory();

                /*onnectionString = "Server=localhost;Port=5432;Database=test;Username=test;Password=password;Timeout=1000;SslMode=require;Ssl=true"*/
            }
        }

    }
}

/*"Server=fantasystocktradingapp-hcuellar-postgresql.postgres.database.azure.com;
 * Database=stocktradingapp;
 * Port=5432;
 * User Id=postgreSQLAdmin@fantasystocktradingapp-hcuellar-postgresql;
 * Password=rSzVdC2M7rGfTmVc;
 * Ssl Mode=Require;"*/


/*
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
 */
