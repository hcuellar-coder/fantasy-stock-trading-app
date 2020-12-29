using FantasyStockTradingApp.Core.Entities;
using FantasyStockTradingApp.Core.Services;
using Microsoft.AspNetCore.Mvc;
using NHibernate;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public void NewAccount_ShouldCreateAccountAsync()
        {
            var UserId = 3;
            var account = new List<Account>
            {
                new Account
                {
                    Id = 1,
                    UserId = 3,
                    Balance = 10000,
                    PortfolioBalance = 0
                }
            };

            ITransaction transaction = _session.BeginTransaction();
            transaction.CommitAsync();
            _session.SaveAsync(account);

            _sut.NewAccount(UserId);

        }
    }
}