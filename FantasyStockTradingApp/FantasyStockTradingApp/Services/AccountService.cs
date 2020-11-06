﻿using System;
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
        Task NewAccount(int user_id, float balance, float portfolio_balance);
        Task UpdateBalance(int account_id, float balance, float portfolio_balance);
    }

    public class AccountService : IAccountService
    {
        private readonly ISession _session;

        public AccountService(ISession session)
        {
            _session = session;
        }

        public async Task NewAccount(int user_id, float balance, float portfolio_balance)
        {
            Console.WriteLine("account_id = " + user_id);
            Console.WriteLine("balance = " + balance);
            Console.WriteLine("portfolio_balance = " + portfolio_balance);

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
                }
            }
            catch (Exception ex)
            {
                var errorString = $"Error inserting user: { ex }";
                throw new Exception(errorString);
            }

        }

        public async Task UpdateBalance(int account_id, float balance, float portfolio_balance)
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
        }

    }
}