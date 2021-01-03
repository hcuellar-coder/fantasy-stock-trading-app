using FantasyStockTradingApp.Core.Entities;
using FantasyStockTradingApp.Core.Services;
using NHibernate;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FantasyStockTradingApp.Tests
{
    [TestFixture]
    class UserServiceTests
    {
        private INHibernateService _nHibernateService;
        private ISession _session;
        private IUserService _sut;

        [SetUp]
        public void Setup()
        {
            _nHibernateService = Substitute.For<INHibernateService>();
            _session = Substitute.For<ISession>();
            _nHibernateService.OpenSession().Returns(_session);
            _sut = new UserService(_nHibernateService);
        }

        [Test]
        public void GetUser_ShouldReturnUsert()
        {
            int id = 2;
            string email = "test@tesing.com";
            string password = "password";
            string firstName = "firstName";
            string lastName = "lastName";


            var user = new List<User>
            {
                new User
                {
                    Id = id,
                    Email = email,
                    Password = password,
                    FirstName = firstName,
                    LastName = lastName
                }
            };

            _session.Query<User>().Returns(user.AsQueryable());

            var resultUser = _sut.GetUser(email);

            Assert.That(resultUser.Count, Is.EqualTo(1));
        }

        [Test]
        public void NewUser_ShouldCreateUserAsync()
        {
            string email = "test@tesing.com";
            string password = "password";
            string firstName = "firstName";
            string lastName = "lastName";

            _sut.NewUser(email, password, firstName, lastName);
            _session.Received(1).SaveAsync(Arg.Any<User>());
        }

        [Test]
        public void isValidUser_ShouldReturnTrue()
        {
            int id = 2;
            string email = "test@tesing.com";
            string password = "password";
            string firstName = "firstName";
            string lastName = "lastName";

            var user = new List<User>
            {
                new User
                {
                    Id = id,
                    Email = email,
                    Password = password,
                    FirstName = firstName,
                    LastName = lastName
                }
            };

            //_session.QueryOver<Holdings>().Returns(holdings.ToList<Holdings>);
            //_session.Query<Holdings>().Returns(holdings.AsQueryable());
            _session.Query<User>().Returns(user.AsQueryable());

            bool result = _sut.isValidUser(email, password);
            Assert.That(result, Is.EqualTo(true));

        }

    }
}
