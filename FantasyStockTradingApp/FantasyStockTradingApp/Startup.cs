using FantasyStockTradingApp.Services;
using FantasyStockTradingApp.SessionFactories;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NHibernate;
using NHibernate.NetCore;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Configuration;

namespace FantasyStockTradingApp
{
    public class Startup
    {
        public IConfiguration _configuration;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public Startup(IConfiguration configuration, 
            IWebHostEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            /*var TOKEN = "";
            if (_hostingEnvironment.IsDevelopment())
            {
                TOKEN = ConfigurationManager.AppSettings["token"];
            }
            else
            {
                TOKEN = Environment.GetEnvironmentVariable("token");
            }*/

            services.AddHttpClient("iexCloud", c =>
            {
                c.BaseAddress = new Uri("https://cloud.iexapis.com/v1/");
                /*c.DefaultRequestHeaders.Add("Authorization", "token " + TOKEN);*/
            });

            services.AddControllers();
            services.AddControllersWithViews();
            services.AddControllers().AddNewtonsoftJson();

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "client/build";
            });

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IIexCloudService, IexCloudService>();
            services.AddScoped<ITransactionService, TransactionService>();

            var _sessionFactory = Fluently.Configure()
               .Database(PostgreSQLConfiguration.Standard.ConnectionString(_configuration.GetConnectionString("DefaultConnection")))
               .Mappings(m => m.FluentMappings.AddFromAssembly(GetType().Assembly))
               .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(false, true))
               .BuildSessionFactory();

            services.AddScoped(f =>
            {
                return _sessionFactory.OpenSession();
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "client";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}
