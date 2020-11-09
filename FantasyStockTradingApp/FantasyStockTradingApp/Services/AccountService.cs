using System;
using NHibernate;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FantasyStockTradingApp.Models;
using FantasyStockTradingApp.Configuration;
using NHibernate.Linq;
using System.Security.AccessControl;
using NHibernate.SqlCommand;
using NHibernate.Impl;

namespace FantasyStockTradingApp.Services
{
    public interface IAccountService
    {
        IQueryable<Account> GetAccount(int user_id);
        Task<Account> NewAccount(int user_id);
        Task UpdateAccount(int account_id, float balance, float portfolio_balance);
    }

    public class AccountService : IAccountService
    {
        private readonly ISession _session;

        public AccountService()
        {
            _session = NHibernateHelper.GetCurrentSession();
        }

        public IQueryable<Account> GetAccount(int user_id)
        {
            try
            {
                using (ITransaction transaction = _session.BeginTransaction())
                {
                    var result = _session.Query<Account>()
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
                NHibernateHelper.CloseSession();
            }
        }

        public async Task<Account> NewAccount(int user_id)
        {
            Console.WriteLine("account_id = " + user_id);

            try
            {
                using (ITransaction transaction = _session.BeginTransaction())
                {
                    var account = new Account
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
                NHibernateHelper.CloseSession();
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
                        "portfolio_balance =:portfolio_balance where account_id =:account_id");
                    query.SetParameter("balance", balance);
                    query.SetParameter("portfolio_balance", portfolio_balance);
                    query.SetParameter("account_id", account_id);
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
                NHibernateHelper.CloseSession();
            }
        }

    }
}