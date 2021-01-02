using FantasyStockTradingApp.Core.Entities;
using FantasyStockTradingApp.Core.Services;
using FantasyStockTradingApp.Core.Exceptions;
using NHibernate;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace FantasyStockTradingApp.Tests
{
    [TestFixture]
    class HoldingServiceTests
    {
        private INHibernateService _nHibernateService;
        private IIexCloudService _iexCloudService;
        private ISession _session;
        private IHoldingsService _sut;

        [SetUp]
        public void Setup()
        {
            _nHibernateService = Substitute.For<INHibernateService>();
            _iexCloudService = Substitute.For<IIexCloudService>();
            _session = Substitute.For<ISession>();
            _nHibernateService.OpenSession().Returns(_session);
            _sut = new HoldingsService(_nHibernateService, _iexCloudService);
        }

        [Test]
        public void GetHoldings_ShouldReturnHoldings()
        {
            var AccountId = 2;
            float LatestCostPerStock = 10.65F;
            float Change = -0.33F;
            float ChangePercentage = -0.02957F;
            var LastUpdated = DateTime.Now;

            var holdings = new List<Holdings>
            {
                new Holdings
                {
                    Id = 2,
                    AccountId = AccountId,
                    CompanyName = "General Electric Co.",
                    Symbol = "GE",
                    StockCount = 5,
                    LatestCostPerStock = LatestCostPerStock,
                    Change = Change,
                    ChangePercentage = ChangePercentage,
                    LastUpdated = LastUpdated,
                }
            };

            _session.Query<Holdings>().Returns(holdings.AsQueryable());

            var resultHoldings = _sut.GetHoldings(AccountId);
            Console.WriteLine(resultHoldings);
            Assert.That(resultHoldings.Count, Is.EqualTo(1));

            //_session.Received(1).SaveAsync(Arg.Is<Account>(a => a.Id == 0 && a.UserId == 3 && a.Balance == 100000));
        }

        
    }
}
