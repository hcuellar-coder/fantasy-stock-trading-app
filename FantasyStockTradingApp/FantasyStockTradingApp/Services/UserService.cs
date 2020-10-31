using FantasyStockTradingApp.Models;
using Microsoft.AspNetCore.Http;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FantasyStockTradingApp.Services
{

    public interface IUserService
    {
        IQueryable<User> GetUserInformation(string email, string password);
        void AddNewUser(string email, string password, string first_name, string last_name);
    }
    public class UserService : IUserService
    {
        private readonly ISessionFactory _sessionFactory;

        public UserService(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }
        public IQueryable<User> GetUserInformation(string email, string password)
        {
            try
            {
                using (var session = _sessionFactory.OpenSession())
                {
                    var result = session.Query<User>().Where(user => user.Email == email && user.Password == password);
                    return result;
                }
            }
            catch ( Exception ex)
            {
                var errorString = $"User does not exist: { ex }";
                throw new Exception(errorString);
            }

        }

        public void AddNewUser(string email, string password, string first_name, string last_name)
        {
            try
            {
                using (var session = _sessionFactory.OpenSession())
                {
                    var user = new User
                    {
                        Email = email,
                        Password = password,
                        First_name = first_name,
                        Last_name = last_name
                    };
                    session.Save(user);
                }
            }
            catch (Exception ex)
            {
                var errorString = $"Error inserting user: { ex }";
                throw new Exception(errorString);
            }
            finally
            {
                
            }

        }

    }
}
