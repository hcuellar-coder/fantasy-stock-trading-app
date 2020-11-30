using NUnit.Framework;
using NSubstitute;
using NHibernate;
using FantasyStockTradingApp.Core.Entities;
using FantasyStockTradingApp.Core.Services;
using System.Linq;
using System.Collections.Generic;
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
            var returnData = new Account
            {
                Id = 2,
                UserId = 2,
                Balance = 10000,
                PortfolioBalance = 0
            };

            _sut.GetAccount(UserId).Returns((IQueryable<Account>)returnData);

            
            //Assert.That()
        }
    }
}


/*
             _sut.GetAccount(UserId)
                .Returns(new List<Account>
                {
                    new Account
                    {
                        Id = 2,
                        UserId = 2,
                        Balance = 10000,
                        PortfolioBalance = 0
                    }
                });
             * */