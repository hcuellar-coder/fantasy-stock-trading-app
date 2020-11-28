using FantasyStockTradingApp.Core.Entities;
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

namespace FantasyStockTradingApp.Core.Services
{

    public interface ITransactionService
    {
        Task NewTransaction(int account_id, string type, string symbol, int stock_count,
                            float cost_per_stock, float cost_per_transaction, DateTime transaction_date);
    }
    public class TransactionService : ITransactionService
    {

        private readonly ISession _session;
        private readonly INHibernateService _nHibernateService;

        public TransactionService(INHibernateService nHibernateService)
        {
            _nHibernateService = nHibernateService;
            _session = _nHibernateService.OpenSession();
        }

        public async Task NewTransaction(int account_id, string type, string symbol, int stock_count,
                                     float cost_per_stock, float cost_per_transaction, DateTime transaction_date) { 
            Console.WriteLine("cost_per_stock = " + cost_per_stock);
            Console.WriteLine("cost_per_transaction = " + cost_per_transaction);
            Console.WriteLine("stock_count = " + stock_count);
            Console.WriteLine("symbol = " + symbol);
            Console.WriteLine("type = " + type);
            Console.WriteLine("transaction_date = " + transaction_date);
            try
            {
                using (ITransaction transaction = _session.BeginTransaction())
                {
                    var stock_transaction = new Transaction
                    {
                        Account_Id = account_id,
                        Type = type,
                        Symbol = symbol,
                        Stock_Count = stock_count,
                        Cost_per_Stock = cost_per_stock,
                        Cost_per_Transaction = cost_per_transaction,
                        Transaction_Date = transaction_date

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
                _nHibernateService.CloseSession();
            }
        }

    }
}
