﻿using NHibernate.Id.Insert;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FantasyStockTradingApp.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;


namespace FantasyStockTradingApp.Services
{
    public interface IHoldingsService
    {
        IQueryable<Holdings> GetHoldings(int account_id);
        Task NewHolding(int account_id, string symbol, int stock_count,
                float latest_cost_per_stock, string last_Updated);
        Task UpdateHolding(int account_id, string symbol, int stock_count, 
                        float latest_cost_per_stock, string last_Updated);
        Task UpdateHoldings(JObject data);
        Task DeleteHolding(int account_id, string symbol);
    }

    public class HoldingsService : IHoldingsService
    {
        private readonly ISession _session;

        public HoldingsService(ISession session)
        {
            _session = session;
        }

        public IQueryable<Holdings> GetHoldings(int account_id)
        {
            try
            {
                using (ITransaction transaction = _session.BeginTransaction())
                {
                    var result = _session.Query<Holdings>()
                        .Where(holdings => holdings.Account_Id == account_id);
                    return result;
                }
            }
            catch (Exception ex)
            {
                var errorString = $"User does not exist: { ex }";
                throw new Exception(errorString);
            }
        }

        public async Task NewHolding(int account_id, string symbol, int stock_count,
                        float latest_cost_per_stock, string last_Updated)
        {
            try
            {
                using (ITransaction transaction = _session.BeginTransaction())
                {
                    var holdings = new Holdings
                    {
                        Account_Id = account_id,
                        Symbol = symbol,
                        Stock_Count = stock_count,
                        Latest_cost_per_stock = latest_cost_per_stock,
                        Last_Updated = last_Updated,
                    };


                    await _session.SaveAsync(holdings);
                    await transaction.CommitAsync();
                }
            }
            catch (Exception ex)
            {
                var errorString = $"Error inserting user: { ex }";
                throw new Exception(errorString);
            }
        }

        public async Task UpdateHolding(int account_id, string symbol, int stock_count,
                        float latest_cost_per_stock, string last_Updated)
        {
            Console.WriteLine("account_id = " + account_id);
            Console.WriteLine("symbol = " + symbol);
            Console.WriteLine("stock_count = " + stock_count);
            Console.WriteLine("latest_cost_per_stock = " + latest_cost_per_stock);
            Console.WriteLine("last_Updated = " + last_Updated);

            try
            {
                using (ITransaction transaction = _session.BeginTransaction())
                {
                   
                    var query = _session.CreateQuery("Update Holdings set stock_count =:stock_count, " +
                    "latest_cost_per_stock =:latest_cost_per_stock, last_Updated =:last_Updated " +
                    "where account_id =:account_id and symbol =:symbol");
                    query.SetParameter("stock_count", stock_count);
                    query.SetParameter("latest_cost_per_stock", latest_cost_per_stock);
                    query.SetParameter("last_Updated", last_Updated);
                    query.SetParameter("account_id", account_id);
                    query.SetParameter("symbol", symbol);

                    await query.ExecuteUpdateAsync();
                    await transaction.CommitAsync();

                }
            }
            catch (Exception ex)
            {
                var errorString = $"Error inserting user: { ex }";
                throw new Exception(errorString);
            }
        }

        public async Task UpdateHoldings(JObject data)
        {
            /*var account_id = Int32.Parse(data["account_id"].ToString());
            var symbol = data["symbol"].ToString();
            var stock_count = Int32.Parse(data["stock_count"].ToString());
            var latest_cost_per_stock = float.Parse(data["latest_cost_per_stock"].ToString());
            var last_Updated = data["updated_time"].ToString();*/

            /*Console.WriteLine("account_id = " + account_id);
            Console.WriteLine("symbol = " + symbol);
            Console.WriteLine("latest_cost_per_stock = " + latest_cost_per_stock);
            Console.WriteLine("last_Updated = " + last_Updated);*/
            
            try
            {
                using (ITransaction transaction = _session.BeginTransaction())
                {
                    dynamic holding_data = JsonConvert.DeserializeObject((string)data);
                    foreach (var holding in holding_data)
                    {
                       /* var symbol = holding["symbol"].ToString();
                        var stock_count = Int32.Parse(data["stock_count"].ToString());
                        var latest_cost_per_stock = float.Parse(data["latest_cost_per_stock"].ToString());
                        var last_Updated = data["updated_time"].ToString(); */

                        var query = _session.CreateQuery("Update Holdings set latest_cost_per_stock =:latest_cost_per_stock, " +
                            "last_Updated =:last_Updated where account_id =:account_id and symbol =:symbol");
                        query.SetParameter("latest_cost_per_stock", holding_data.latest_cost_per_stock);
                        query.SetParameter("last_Updated", holding_data.last_Updated);
                        query.SetParameter("account_id", holding_data.account_id);
                        query.SetParameter("symbol", holding_data.symbol);

                        await query.ExecuteUpdateAsync();
                        await transaction.CommitAsync();
                    }
                        
                }
            }
            catch (Exception ex)
            {
                var errorString = $"Error inserting user: { ex }";
                throw new Exception(errorString);
            }
        }

        public async Task DeleteHolding(int account_id, string symbol)
        {
            try
            {
                using (ITransaction transaction = _session.BeginTransaction())
                {
                    var query = _session.CreateQuery("Delete from Holdings " +
                        "where account_id =:account_id and symbol =:symbol");
                    query.SetParameter("account_id", account_id);
                    query.SetParameter("symbol", symbol);

                    await query.ExecuteUpdateAsync();
                    await transaction.CommitAsync();
                }
            }
            catch (Exception ex)
            {
                var errorString = $"Error inserting user: { ex }";
                throw new Exception(errorString);
            }
        }
    }
}