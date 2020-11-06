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
        Task UpdateHoldings(int account_id, string symbol, int stock_count, 
                        float latest_cost_per_stock, string updated_time);
    }

    public class HoldingsService : IHoldingsService
    {
        private readonly ISession _session;

        public HoldingsService(ISession session)
        {
            _session = session;
        }

        public async Task UpdateHoldings(int account_id, string symbol, int stock_count,
                        float latest_cost_per_stock, string last_Updated)
        {
            Console.WriteLine("account_id = " + account_id);
            Console.WriteLine("symbol = " + symbol);
            Console.WriteLine("stock_count = " + stock_count);
            Console.WriteLine("latest_cost_per_stock = " + latest_cost_per_stock);
            Console.WriteLine("updated_time = " + last_Updated);
            
            
            
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

    }
}
