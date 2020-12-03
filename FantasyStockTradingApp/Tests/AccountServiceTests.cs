using NUnit.Framework;
using NSubstitute;
using NHibernate;
using FantasyStockTradingApp.Core.Entities;
using FantasyStockTradingApp.Core.Services;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace Tests
{
    [TestFixture]
    public class AccountServiceTests
    {
        private INHibernateService _nHibernateService;
        private ISession _session;
        private IAccountService _sut;

        [SetUp]
        public void Setup()
        {
            _nHibernateService = Substitute.For<INHibernateService>();
            _session = _nHibernateService.OpenSession();
            _sut = new AccountService(_nHibernateService);
        }

        [Test]
        public void GetAccount_ShouldReturnAccount()
        {
            var UserId = 2;
            var accounts = new List<Account>
            {
                new Account
                {
                    Id = 2,
                    UserId = 2,
                    Balance = 10000,
                    PortfolioBalance = 0
                }
            };

            _session.Query<Account>().Returns(accounts.AsQueryable());

            var resultAccounts = _sut.GetAccount(UserId);

            Assert.That(resultAccounts.Count, Is.EqualTo(1));
        }

        [Test]
        public void GetAccount_ShouldThrowException()
        {
            var UserId = 3;
            var accounts = new List<Account>
            {
                new Account
                {
                    Id = 2,
                    UserId = 2,
                    Balance = 10000,
                    PortfolioBalance = 0
                }
            };

            //_session.Query<Account>().Returns(accounts.AsQueryable());

            //_session.Query<Account>().Returns(new Exception("user not found"));
            //Need to fix this!
            //var resultAccounts = _sut.GetAccount(UserId);
            Assert.Throws<ArgumentException>(() => _sut.GetAccount(UserId));
            //Assert.Throws<Exception>(() => _sut.GetAccount(UserId));

            //Assert.Throws<InvalidOperationException>(() => _sut.GetAccount(UserId));

            //Assert.AreEqual("")

           //Assert.Throws(typeof(ArgumentException), delegate { throw new ArgumentException(); });
           
        }

        [Test]
        public void NewAccount_ShouldCreateAccount()
        {
            var UserId = 2;
            var accounts = new List<Account>
            {
                new Account
                {
                    Id = 2,
                    UserId = 2,
                    Balance = 10000,
                    PortfolioBalance = 0
                }
            };

            //_session.Query<Account>().Returns(accounts.AsQueryable());
            //_session.SaveAsync(accounts);

            //await _session.CommitAsync();

            //var resultAccounts = 

            _sut.NewAccount(UserId);
            //_session.Query<Account>().Returns(accounts);

            //var resultAccounts = _sut.GetAccount(UserId);

            //Assert.That(resultAccounts, Is.EqualTo(accounts));
        }
    }
}