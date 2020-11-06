using FantasyStockTradingApp.Models;
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

namespace FantasyStockTradingApp.Services
{

    public interface IUserService
    {
        IQueryable<User> GetUser(string email, string password);
        Task NewUser(string email, string password, string first_name, string last_name);
    }
    public class UserService : IUserService
    {

        private readonly ISession _session;

        public UserService(ISession session)
        {
            _session = session;
        }


        public IQueryable<User> GetUser(string email, string password)
        {
            Console.WriteLine("email = "+ email);
            Console.WriteLine("password = "+ password);
            try
            {
                using (ITransaction transaction = _session.BeginTransaction())
                {
                    var result = _session.Query<User>()
                        .Where(user => user.Email == email && user.Password == password);
                    return result;
                }
            }
            catch ( Exception ex)
            {
                var errorString = $"User does not exist: { ex }";
                throw new Exception(errorString);
            }

        }

        public async Task NewUser(string email, string password, string first_name, string last_name)
        {
            Console.WriteLine("email = "+ email);
            Console.WriteLine("password = "+ password);
            Console.WriteLine("first_name = "+ first_name);
            Console.WriteLine("last_name = "+ last_name);
            try
            {
                using (ITransaction transaction = _session.BeginTransaction())
                {
                    var user = new User
                    {
                        Email = email,
                        Password = password,
                        First_name = first_name,
                        Last_name = last_name
                    };
                    await _session.SaveAsync(user);
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
