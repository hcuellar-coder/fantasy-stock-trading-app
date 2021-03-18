using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Reflection;

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
        private readonly IConfiguration _configuration;

        public NHibernateService(IWebHostEnvironment hostingEnvironment, IConfiguration configuration)
        {
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
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
            return Fluently.Configure()
               .Database(PostgreSQLConfiguration.PostgreSQL82
               .ConnectionString(c => c
                .Host(_configuration["Database:Host"])
                .Port(int.Parse(_configuration["Database:Port"]))
                .Database(_configuration["Database:DbName"])
                .Username(_configuration["Database:Username"])
                .Password(_configuration["Database:Password"])))
               .Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.GetExecutingAssembly()))
               .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(false, true))
               .BuildSessionFactory();


            // if (_hostingEnvironment.EnvironmentName == "Development")
            // {
            //     return Fluently.Configure()
            //    .Database(PostgreSQLConfiguration.PostgreSQL82
            //    .ConnectionString(c =>
            //         c.Host("localhost")
            //         .Port(5432)
            //         .Database("FantasyStockTradingApp")
            //         .Username("postgres")
            //         .Password("postgres")))
            //    .Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.GetExecutingAssembly()))
            //    .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(false, true))
            //    .BuildSessionFactory();
            // } else
            // {
            //    var DBPASSWORD = Environment.GetEnvironmentVariable("DBPASSWORD");
            //     return Fluently.Configure()
            //    .Database(PostgreSQLConfiguration.PostgreSQL82
            //    .ConnectionString(c =>
            //         c.Host("fantasystocktrader-hcuellar-postgresql.cukdhjozet53.us-east-2.rds.amazonaws.com")
            //         .Port(5432)
            //         .Database("postgres")
            //         .Username("postgreSQLAdmin")
            //         .Password(DBPASSWORD)
            //         )
            //    )
            //    .Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.GetExecutingAssembly()))
            //    .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(false, true))
            //    .BuildSessionFactory();
            // }
        }
    }
}