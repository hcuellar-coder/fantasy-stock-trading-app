﻿using FantasyStockTradingApp.Core.Entities;
using NHibernate;
using System.Linq;
using System.Threading.Tasks;
using EmailValidation;
using System.IO;
using FantasyStockTradingApp.Core.Exceptions;

namespace FantasyStockTradingApp.Core.Services
{
    public interface IUserService
    {
        bool isValidUser(string Email, string? Password = null);
        IQueryable<User> GetUser(string Email);
        Task NewUser(string Email, string Password, string FirstName, string LastName);
    }
    public class UserService : IUserService
    {
        private readonly ISession _session;
        private readonly INHibernateService _nHibernateService;
        private readonly string _path;


        public UserService(INHibernateService nHibernateService)
        {
            _nHibernateService = nHibernateService;
            _session = _nHibernateService.OpenSession();
            _path = Path.GetFullPath(ToString());

        }

        public bool isValidUser(string Email, string? Password = null)
        {
            try
            {
                using (ITransaction transaction = _session.BeginTransaction())
                {

                    if (Password == null)
                    {
                        var result = _session.Query<User>()
                        .Where(user_login => user_login.Email == Email)
                        .Count() > 0;
                        return result;
                    }
                    else
                    {
                        var result = _session.Query<User>()
                        .Where(user_login => user_login.Email == Email && user_login.Password == Password)
                        .Count() > 0;
                        return result;
                    }
                }
            }
            catch
            {
                throw new IsUserValidException(_path, "isValidUser()");
            }
            finally
            {
                _nHibernateService.CloseSession();
            }
        }

        public IQueryable<User> GetUser(string Email)
        {
            try
            {
                using (ITransaction transaction = _session.BeginTransaction())
                {
                    var result = _session.Query<User>()
                    .Where(user => user.Email == Email);
                    return result;
                }
            }
            catch
            {
                throw new GetUserException(_path, "GetUser()");
            }
            finally
            {
                _nHibernateService.CloseSession();
            }

        }
       
        public async Task NewUser(string Email, string Password, string FirstName, string LastName)
        {
            
                if (EmailValidator.Validate(Email))
                {
                    try
                    {
                        using (ITransaction transaction = _session.BeginTransaction())
                        {
                            var user = new User
                            {
                                Email = Email,
                                Password = Password,
                                FirstName = FirstName,
                                LastName = LastName
                            };
                            await _session.SaveAsync(user);
                            await transaction.CommitAsync();
                        }
                    }
                    catch
                    {
                        throw new NewUserInsertException(_path, "NewUser()");
                    }
                    finally
                    {
                        _nHibernateService.CloseSession();
                    }
                }
                else
                {
                    throw new NewUserEmailException(_path, "NewUser()");
                }
           
        }
    }
}
