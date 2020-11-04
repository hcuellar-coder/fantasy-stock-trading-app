using FantasyStockTradingApp.Models;
using Microsoft.AspNetCore.Mvc;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Web;
using Microsoft.AspNetCore.Server.HttpSys;
using System.Net;

namespace FantasyStockTradingApp.Services
{

    public interface ITransactionService
    {
        Task StockTransaction(float cost, int stock_count, string symbol, string type);
    }
    public class TransactionService : ITransactionService
    {

        private readonly ISession _session;

        public TransactionService(ISession session)
        {
            _session = session;
        }

        public async Task StockTransaction(float cost, int stock_count, string symbol, string type)
        {
            Console.WriteLine("cost = " + cost);
            Console.WriteLine("stock_count = " + stock_count);
            Console.WriteLine("symbol = " + symbol);
            Console.WriteLine("type = " + type);
            try
            {
                using (ITransaction transaction = _session.BeginTransaction())
                {
                    var stock_transaction = new Transaction
                    {
                        Cost = cost,
                        Stock_count = stock_count,
                        Symbol = symbol,
                        Type = type
                    };
                    await _session.SaveAsync(stock_transaction);
                    await transaction.CommitAsync();
                }
            }
            catch (Exception ex)
            {
                var errorString = $"Error inserting user: { ex }";
                throw new Exception(errorString);
            }
            finally
            {
                
            }

        }

    }
}
