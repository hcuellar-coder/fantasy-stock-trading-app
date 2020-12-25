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
using System.IO;
using FantasyStockTradingApp.Core.Exceptions;

namespace FantasyStockTradingApp.Core.Services
{

    public interface ITransactionService
    {
        Task NewTransaction(int AccountId, string Type, string Symbol, int StockCount,
                            float CostPerStock, float CostPerTransaction, DateTime TransactionDate);
    }
    public class TransactionService : ITransactionService
    {

        private readonly ISession _session;
        private readonly INHibernateService _nHibernateService;
        private readonly string _path;

        public TransactionService(INHibernateService nHibernateService)
        {
            _nHibernateService = nHibernateService;
            _session = _nHibernateService.OpenSession();
            _path = Path.GetFullPath(ToString());
        }

        public async Task NewTransaction(int AccountId, string Type, string Symbol, int StockCount,
                                     float CostPerStock, float CostPerTransaction, DateTime TransactionDate) 
        { 
            try
            {
                using (ITransaction transaction = _session.BeginTransaction())
                {
                    var stock_transaction = new Transaction
                    {
                        AccountId = AccountId,
                        Type = Type,
                        Symbol = Symbol,
                        StockCount = StockCount,
                        CostPerStock = CostPerStock,
                        CostPerTransaction = CostPerTransaction,
                        TransactionDate = TransactionDate

                    };
                    await _session.SaveAsync(stock_transaction);
                    await transaction.CommitAsync();
                }
            }
            catch
            {
                throw new NewTransactionException(_path, "NewTransaction()");
            }
            finally
            {
                _nHibernateService.CloseSession();
            }
        }

    }
}
