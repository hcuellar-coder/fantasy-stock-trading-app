using NHibernate.Id.Insert;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FantasyStockTradingApp.Core.Entities;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using FantasyStockTradingApp.Core.Exceptions;

namespace FantasyStockTradingApp.Core.Services
{
    public interface IHoldingsService
    {
        IQueryable<Holdings> GetHoldings(int AccountId);

        Task NewHolding(int AccountId, string CompanyName, string Symbol, int StockCount,
                        float LatestCostPerStock, float Change, float ChangePercentage, DateTime LastUpdated);
        Task UpdateHolding(int AccountId, string Symbol, int StockCount,
                        float LatestCostPerStock, DateTime LastUpdated);
        Task UpdateHoldings(JObject Data);
        Task DeleteHolding(int AccountId, string Symbol);

        bool HoldingExists(int AccountId, string Symbol);
    }

    public class HoldingsService : IHoldingsService
    {
        private readonly ISession _session;
        private readonly INHibernateService _nHibernateService;
        private readonly IIexCloudService _iexCloudService;
        private readonly string _path;

        public HoldingsService(INHibernateService nHibernateService,
                            IIexCloudService iexCloudService)
        {
            _nHibernateService = nHibernateService;
            _session = _nHibernateService.OpenSession();
            _iexCloudService = iexCloudService;
            _path = Path.GetFullPath(ToString());
        }

        public bool HoldingExists(int AccountId, string Symbol)
        {
            try
            {
                using (ITransaction transaction = _session.BeginTransaction())
                {
                    var result = _session.QueryOver<Holdings>()
                    .Where(holding => holding.AccountId == AccountId && holding.Symbol == Symbol)
                    .RowCount() > 0;
                    return result;
                }
            }
            catch
            {
                throw new HoldingExistsException(_path, "HoldingExists()");
            }
            finally
            {
                _nHibernateService.CloseSession();
            }
        }

        public IQueryable<Holdings> GetHoldings(int AccountId)
        {
            try
            {
                using (ITransaction transaction = _session.BeginTransaction())
                {
                    var result = _session.Query<Holdings>()
                        .Where(holdings => holdings.AccountId == AccountId);
                    return result;
                }
            }
            catch
            {
                throw new GetHoldingException(_path, "GetHoldings()");
            }
            finally
            {
                _nHibernateService.CloseSession();
            }
        }

        public async Task NewHolding(int AccountId, string CompanyName, string Symbol, int StockCount,
            float LatestCostPerStock, float Change, float ChangePercentage, DateTime LastUpdated)
        {
            try
            {
                using (ITransaction transaction = _session.BeginTransaction())
                {
                    var holdings = new Holdings
                    {
                        AccountId = AccountId,
                        CompanyName = CompanyName,
                        Symbol = Symbol,
                        StockCount = StockCount,
                        LatestCostPerStock = LatestCostPerStock,
                        Change = Change,
                        ChangePercentage = ChangePercentage,
                        LastUpdated = LastUpdated,
                    };

                    await _session.SaveAsync(holdings);
                    await transaction.CommitAsync();
                }
            }
            catch
            {
                throw new NewHoldingException(_path, "NewHolding()");
            }
            finally
            {
                _nHibernateService.CloseSession();
            }
        }

        public async Task UpdateHolding(int AccountId, string Symbol, int StockCount,
                        float LatestCostPerStock, DateTime LastUpdated)
        {
            try
            {
                using (ITransaction transaction = _session.BeginTransaction())
                {
                   
                    var query = _session.CreateQuery("Update Holdings set stock_count =:StockCount, " +
                    "latest_cost_per_stock =:LatestCostPerStock, last_Updated =:LastUpdated " +
                    "where account_id =:AccountId and symbol =:Symbol");
                    query.SetParameter("StockCount", StockCount);
                    query.SetParameter("LatestCostPerStock", LatestCostPerStock);
                    query.SetParameter("LastUpdated", LastUpdated);
                    query.SetParameter("AccountId", AccountId);
                    query.SetParameter("Symbol", Symbol);

                    await query.ExecuteUpdateAsync();
                    await transaction.CommitAsync();

                }
            }
            catch
            {
                throw new UpdateHoldingException(_path, "UpdateHolding()");
            }
            finally
            {
                _nHibernateService.CloseSession();
            }
        }

        public async Task UpdateHoldings(JObject Holdings)
        {

            try
            {
                foreach (var holding in Holdings["Holdings"])
                {
                    var quote = await _iexCloudService.GetQuote(holding["symbol"].ToString());
                    var AccountId = Int32.Parse(holding["accountId"].ToString());
                    var Symbol = holding["symbol"].ToString();
                    var StockCount = Int32.Parse(holding["stockCount"].ToString());
                    var LatestCostPerStock = float.Parse(quote.LatestPrice.ToString());
                    var LastUpdated = DateTime.Now;

                    using (ITransaction transaction = _session.BeginTransaction())
                    {

                        var query = _session.CreateQuery("Update Holdings set stock_count =:StockCount, " +
                        "latest_cost_per_stock =:LatestCostPerStock, last_Updated =:LastUpdated " +
                        "where account_id =:AccountId and symbol =:Symbol");
                        query.SetParameter("StockCount", StockCount);
                        query.SetParameter("LatestCostPerStock", LatestCostPerStock);
                        query.SetParameter("LastUpdated", LastUpdated);
                        query.SetParameter("AccountId", AccountId);
                        query.SetParameter("Symbol", Symbol);

                        await query.ExecuteUpdateAsync();
                        await transaction.CommitAsync();

                    }
                }
            }
            catch
            {
                throw new UpdateHoldingsException(_path, "UpdateHoldings()");
            }
            finally
            {
                _nHibernateService.CloseSession();
            }
        }

        public async Task DeleteHolding(int AccountId, string Symbol)
        {
            try
            {
                using (ITransaction transaction = _session.BeginTransaction())
                {
                    var query = _session.CreateQuery("Delete from Holdings " +
                        "where account_id =:AccountId and symbol =:Symbol");
                    query.SetParameter("AccountId", AccountId);
                    query.SetParameter("Symbol", Symbol);

                    await query.ExecuteUpdateAsync();
                    await transaction.CommitAsync();
                }
            }
            catch
            {
                throw new DeleteHoldingsException(_path, "DeleteHolding()");
            }
            finally
            {
                _nHibernateService.CloseSession();
            }
        }
    }
}
