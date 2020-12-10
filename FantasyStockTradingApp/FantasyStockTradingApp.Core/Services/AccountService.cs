using System;
using NHibernate;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FantasyStockTradingApp.Core.Entities;
using NHibernate.Linq;
using System.Security.AccessControl;
using NHibernate.SqlCommand;
using NHibernate.Impl;

namespace FantasyStockTradingApp.Core.Services
{
    public interface IAccountService
    {
        IQueryable<Account> GetAccount(int UserId);
        Task NewAccount(int UserId);
        Task UpdateAccount(int AccountId, float Balance, float PortfolioBalance);
    }

    public class AccountService : IAccountService
    {
        private readonly ISession _session;
        private readonly INHibernateService _nHibernateService;

        public AccountService(INHibernateService nHibernateService)
        {
            _nHibernateService = nHibernateService;
            _session = _nHibernateService.OpenSession();
        }

        public IQueryable<Account> GetAccount(int UserId)
        {
            try
            {
                using (ITransaction transaction = _session.BeginTransaction())
                {
                    var result = _session.Query<Account>()
                        .Where(account => account.UserId == UserId);

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

        public async Task NewAccount(int UserId)
        {
            try
            {
                using (ITransaction transaction = _session.BeginTransaction())
                {
                    var account = new Account
                    {
                        UserId = UserId,
                        Balance = 100000,
                        PortfolioBalance = 0
                    };

                    await _session.SaveAsync(account);
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

        public async Task UpdateAccount(int AccountId, float Balance, float PortfolioBalance)
        {
            try
            {
                using (ITransaction transaction = _session.BeginTransaction())
                { 
                    var query =  _session.CreateQuery("Update Account set balance =:Balance, " +
                        "portfolio_balance =:PortfolioBalance where id =:Id");
                    query.SetParameter("Balance", Balance);
                    query.SetParameter("PortfolioBalance", PortfolioBalance);
                    query.SetParameter("Id", AccountId);
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