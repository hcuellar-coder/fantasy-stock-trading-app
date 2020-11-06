using NHibernate.Id.Insert;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FantasyStockTradingApp.Models;

namespace FantasyStockTradingApp.Services
{
    public interface IHoldingsService
    {
        IQueryable<Holdings> GetHoldings(int account_id);
        Task UpdateHoldings(int account_id, string symbol, int stock_count, 
                        float latest_cost_per_stock, string updated_time);
        Task NewHolding(int account_id, string symbol, int stock_count,
                        float latest_cost_per_stock, string last_Updated);
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

        public async Task UpdateHoldings(int account_id, string symbol, int stock_count,
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
                    var holdingExists = _session.QueryOver<Holdings>()
                        .Where(holdings => holdings.Account_Id == account_id && holdings.Symbol == symbol)
                        .RowCount() > 0;

                    if (holdingExists)
                    {
                        var query = _session.CreateQuery("Update Holdings set stock_count =:stock_count, " +
                       "latest_cost_per_stock =:latest_cost_per_stock, last_Updated :=last_Updated " +
                       "where account_id =:account_id and symbol =:symbol");
                        query.SetParameter("stock_count", stock_count);
                        query.SetParameter("latest_cost_per_stock", latest_cost_per_stock);
                        query.SetParameter("last_Updated", last_Updated);
                        query.SetParameter("account_id", account_id);
                        query.SetParameter("symbol", symbol);

                        await query.ExecuteUpdateAsync();
                        await transaction.CommitAsync();

                    } else
                    {
                      await NewHolding(account_id, symbol, stock_count,
                         latest_cost_per_stock, last_Updated);
                    }                  
                }
            }
            catch (Exception ex)
            {
                var errorString = $"Error inserting user: { ex }";
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
