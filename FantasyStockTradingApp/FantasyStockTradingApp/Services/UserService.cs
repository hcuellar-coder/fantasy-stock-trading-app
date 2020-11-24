using FantasyStockTradingApp.Models;
using FantasyStockTradingApp.Configuration;
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
using FluentNHibernate.Conventions;

namespace FantasyStockTradingApp.Services
{

    public interface IUserService
    {
        IQueryable<User> GetUser(string email);
        Task<User> NewUser(string email, string first_name, string last_name);
    }
    public class UserService : IUserService
    {
        private readonly ISession _session;
        

        public UserService()
        {
            _session = NHibernateHelper.GetCurrentSession();
            
        }

        public IQueryable<User> GetUser(string email)
        {
            Console.WriteLine("email = "+ email);

            try
            {
                using (ITransaction transaction = _session.BeginTransaction())
                {
                    var result = _session.Query<User>()
                    .Where(user => user.email == email);
                    return result;
                }
            }
            catch ( Exception ex)
            {
                var errorString = $"User does not exist: { ex }";
                throw new Exception(errorString);
            }
            finally
            {
                NHibernateHelper.CloseSession();
            }

        }
       
        public async Task<User> NewUser(string email, string first_name, string last_name)
        {
            Console.WriteLine("email = "+ email);
            Console.WriteLine("first_name = "+ first_name);
            Console.WriteLine("last_name = "+ last_name);
            try
            {
                using (ITransaction transaction = _session.BeginTransaction())
                {
                    var user = new User
                    {
                        email = email,
                        first_name = first_name,
                        last_name = last_name
                    };
                    await _session.SaveAsync(user);
                    await transaction.CommitAsync();
                    return user;
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
