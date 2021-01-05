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
using System.IO;
using FantasyStockTradingApp.Core.Exceptions;

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
        private readonly string _path;

        public AccountService(INHibernateService nHibernateService)
        {
            _nHibernateService = nHibernateService;
            _session = _nHibernateService.OpenSession();
            _path = Path.GetFullPath(ToString());
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
            catch
            {
                throw new GetAccountException(_path, "GetAccount()");
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
            catch
            {
                throw new NewAccountException(_path, "NewAccount()");
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
                    var account = _session.Query<Account>().First(h => h.Id == AccountId);

                    account.Balance = Balance;
                    account.PortfolioBalance = PortfolioBalance;

                    //await _session.UpdateAsync(holding);
                    await transaction.CommitAsync();

                }
            }
            catch
            {
                throw new UpdateAccountException(_path, "UpdateAccount()");
            }
            finally
            {
                _nHibernateService.CloseSession();
            }
        }

    }
}