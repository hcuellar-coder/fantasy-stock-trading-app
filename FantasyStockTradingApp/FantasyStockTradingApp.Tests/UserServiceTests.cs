using FantasyStockTradingApp.Core.Entities;
using FantasyStockTradingApp.Core.Exceptions;
using FantasyStockTradingApp.Core.Services;
using NHibernate;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

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
        public void GetUser_ShouldThrowException()
        {
            string email = "test@tesing.com";

            _session.BeginTransaction().ThrowsForAnyArgs(new Exception());
            Assert.Throws<GetUserException>(() => _sut.GetUser(email));
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
        public void NewUser_ShouldThrowInsertException()
        {
            string email = "test@tesing.com";
            string password = "password";
            string firstName = "firstName";
            string lastName = "lastName";

            _session.BeginTransaction().ThrowsForAnyArgs(new Exception());
            Assert.ThrowsAsync<NewUserInsertException>(() => _sut.NewUser(email, password, firstName, lastName));
        }

        [Test]
        public void NewUser_ShouldThrowEmailException()
        {
            string email = "BadEmail";
            string password = "password";
            string firstName = "firstName";
            string lastName = "lastName";

            Assert.ThrowsAsync<NewUserEmailException>(() => _sut.NewUser(email, password, firstName, lastName));
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
            _session.Query<User>().Returns(user.AsQueryable());

            bool result = _sut.isValidUser(email, password);
            Assert.That(result, Is.EqualTo(true));
        }

        [Test]
        public void IsValidUser_ShouldThrowException()
        {
            string email = "test@tesing.com";
            string password = "password";

            _session.BeginTransaction().ThrowsForAnyArgs(new Exception());
            Assert.Throws<IsUserValidException>(() => _sut.isValidUser(email, password));
        }
    }
}
