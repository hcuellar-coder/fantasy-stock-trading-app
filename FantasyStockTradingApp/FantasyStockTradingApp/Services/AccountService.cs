using System;
using NHibernate;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FantasyStockTradingApp.Models;
using NHibernate.Linq;
using System.Security.AccessControl;
using NHibernate.SqlCommand;
using NHibernate.Impl;

namespace FantasyStockTradingApp.Services
{
    public interface IAccountService
    {
        IQueryable<AccountModel> GetAccount(int user_id);
        Task<AccountModel> NewAccount(int user_id);
        Task UpdateAccount(int account_id, float balance, float portfolio_balance);
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

        public IQueryable<AccountModel> GetAccount(int user_id)
        {
            try
            {
                using (ITransaction transaction = _session.BeginTransaction())
                {
                    var result = _session.Query<AccountModel>()
                        .Where(account => account.User_Id == user_id);
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

        public async Task<AccountModel> NewAccount(int user_id)
        {
            Console.WriteLine("account_id = " + user_id);

            try
            {
                using (ITransaction transaction = _session.BeginTransaction())
                {
                    var account = new AccountModel
                    {
                        User_Id = user_id,
                        Balance = 100000,
                        Portfolio_Balance = 0
                    };

                    await _session.SaveAsync(account);
                    await transaction.CommitAsync();
                    return account;
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

        public async Task UpdateAccount(int account_id, float balance, float portfolio_balance)
        {
            Console.WriteLine("account_id = " + account_id);
            Console.WriteLine("balance = " + balance);
            Console.WriteLine("portfolio_balance = " + portfolio_balance);

            try
            {
                using (ITransaction transaction = _session.BeginTransaction())
                { 
                    var query =  _session.CreateQuery("Update Account set balance =:balance, " +
                        "portfolio_balance =:portfolio_balance where id =:id");
                    query.SetParameter("balance", balance);
                    query.SetParameter("portfolio_balance", portfolio_balance);
                    query.SetParameter("id", account_id);
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