using NHibernate.Id.Insert;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FantasyStockTradingApp.Core.Entities;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;


namespace FantasyStockTradingApp.Core.Services
{
    public interface IHoldingsService
    {
        IQueryable<Holdings> GetHoldings(int AccountId);

        Task NewHolding(int AccountId, string CompanyName, string Symbol, int StockCount,
                        float LatestCostPerStock, float Change, float ChangePercentage, DateTime LastUpdated);
        Task UpdateHolding(int AccountId, string Symbol, int StockCount,
                        float LatestCostPerStock, DateTime LastUpdated);
        Task UpdateHoldings(JObject Data); //this should take an account Id and updat
        Task DeleteHolding(int AccountId, string Symbol);

        bool HoldingExists(int AccountId, string Symbol);
    }

    public class HoldingsService : IHoldingsService
    {
        private readonly ISession _session;
        private readonly INHibernateService _nHibernateService;

        public HoldingsService(INHibernateService nHibernateService)
        {
            _nHibernateService = nHibernateService;
            _session = _nHibernateService.OpenSession();
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
            catch (Exception ex)
            {
                var errorString = $"User does not exist: { ex }";
                throw new Exception(errorString);
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
            catch (Exception ex)
            {
                var errorString = $"User does not exist: { ex }";
                throw new Exception(errorString);
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

        public async Task UpdateHoldings(JObject Data)
        {
            try
            {
                using (ITransaction transaction = _session.BeginTransaction())
                {
                    dynamic holding_data = JsonConvert.DeserializeObject((string)Data);
                    foreach (var holding in holding_data)
                    {
                        var query = _session.CreateQuery("Update Holdings set latest_cost_per_stock =:LatestCostPerStock, " +
                            "last_Updated =:LastUpdated where account_id =:AccountId and symbol =:Symbol");
                        query.SetParameter("LatestCostPerStock", holding_data.LatestCostPerStock);
                        query.SetParameter("LastUpdated", holding_data.LastUpdated);
                        query.SetParameter("AccountId", holding_data.AccountId);
                        query.SetParameter("Symbol", holding_data.Symbol);

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
