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
    public class AccountServiceTests
    {
        private INHibernateService _nHibernateService;
        private ISession _session;
        private IAccountService _sut;

        [SetUp]
        public void Setup()
        {
            _nHibernateService = Substitute.For<INHibernateService>();
            _session = Substitute.For<ISession>();
            _nHibernateService.OpenSession().Returns(_session);
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
            var UserId = 2;

            _session.BeginTransaction().ThrowsForAnyArgs(new Exception());
            Assert.Throws<GetAccountException>(() => _sut.GetAccount(UserId));
        }


        [Test]
        public void NewAccount_ShouldCreateAccountAsync()
        {
            var UserId = 3;

            _sut.NewAccount(UserId);
            _session.Received(1).SaveAsync(Arg.Any<Account>());
            //_session.Received(1).SaveAsync(Arg.Is<Account>(a => a.Id == 0 && a.UserId == 3 && a.Balance == 100000));
        }

        [Test]
        public void NewAccount_ShouldThrowException()
        {
            _session.BeginTransaction().ThrowsForAnyArgs(new Exception());
            Assert.ThrowsAsync<NewAccountException>(() => _sut.NewAccount(3));
        }

        [Test]
        public void UpdateAccount_ShouldUpdateAccount()
        {
            int Id = 2;
            int UserId = 2;
            float Balance = 9000F;
            float PortfolioBalance = 1000F;

            var Account = new List<Account>
            {
                new Account
                {
                    Id = Id,
                    UserId = UserId,
                    Balance = 9500F,
                    PortfolioBalance = 500F
                }
            };

            _session.Query<Account>().Returns(Account.AsQueryable());
            _sut.UpdateAccount(Id, Balance, PortfolioBalance);
            Assert.That(Account[0].Balance == 9000F && Account[0].PortfolioBalance == 1000F);

        }

        [Test]
        public void UpdateAccount_ShouldThrowException()
        {
            int Id = 2;
            float Balance = 10000F;
            float PortfolioBalance = 0F;

            _session.BeginTransaction().ThrowsForAnyArgs(new Exception());
            Assert.ThrowsAsync<UpdateAccountException>(() => _sut.UpdateAccount(Id, Balance, PortfolioBalance));
        }
    }
}