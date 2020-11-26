using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NHibernate;
using FantasyStockTradingApp.Models;
using FantasyStockTradingApp.Configuration;

namespace FantasyStockTradingApp.Services
{
    public interface IUserLoginService
    {
        Task NewUserLogin(string email, string password, int user_id);
        bool isValidUser(string email, string? password = null);
    }
    public class UserLoginService : IUserLoginService
    {
        private readonly ISession _session;
        private readonly INHibernateService _nHibernateService;

        public UserLoginService(INHibernateService nHibernateService)
        {
            _nHibernateService = nHibernateService;
            _session = _nHibernateService.OpenSession();
        }

        public bool isValidUser(string email, string? password = null)
        {
            try
            {
                using (ITransaction transaction = _session.BeginTransaction())
                {
                    if (password == null)
                    {
                        var result = _session.QueryOver<UserLogin>()
                        .Where(user_login => user_login.email == email)
                        .RowCount() > 0;
                        return result;
                    }
                    else
                    {
                        var result = _session.QueryOver<UserLogin>()
                        .Where(user_login => user_login.email == email && user_login.password == password)
                        .RowCount() > 0;
                        return result;
                    }
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

        public async Task NewUserLogin(string email, string password, int user_id)
        {
            Console.WriteLine("email = " + email);
            Console.WriteLine("password = " + password);

            try
            {
                using (ITransaction transaction = _session.BeginTransaction())
                {
                    var userLogin = new UserLogin
                    {
                        email = email,
                        password = password,
                        user_account_id = user_id
                    };
                    await _session.SaveAsync(userLogin);
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
