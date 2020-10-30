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
                    /*using (var transaction = session.BeginTransaction())
                    {
                        return await session.
                    }*/
                    //var result = await session.Query<User>().FirstOrDefault();
                    return session.Query<User>().Where(user => user.Email == email && user.Password == password);
                    //return result;
                }
            }
            catch ( Exception ex)
            {
                var errorString = $"User does not exist: { ex }";
                throw new Exception(errorString);
            }

        }

    }
}
