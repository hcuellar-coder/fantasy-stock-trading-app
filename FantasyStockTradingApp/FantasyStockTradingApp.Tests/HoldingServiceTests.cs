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
            Assert.That(resultHoldings.Count, Is.EqualTo(1));
        }

        [Test]
        public void GetHoldings_ShouldReturnTrue()
        {
            int AccountId = 2;
            string Symbol = "GE";
            float LatestCostPerStock = 10.65F;
            float Change = -0.33F;
            float ChangePercentage = -0.02957F;
            var LastUpdated = DateTime.Now;

            /*IList<Holdings> holdings = _session.QueryOver<Holdings>()
                .Where(h => h.AccountId == AccountId)
                .And(h => h.Symbol == Symbol)
                .List();*/

            var holdings = new List<Holdings>
            {
                new Holdings
                {
                    Id = 2,
                    AccountId = AccountId,
                    CompanyName = "General Electric Co.",
                    Symbol = Symbol,
                    StockCount = 5,
                    LatestCostPerStock = LatestCostPerStock,
                    Change = Change,
                    ChangePercentage = ChangePercentage,
                    LastUpdated = LastUpdated
                }
            };

            //_session.QueryOver<Holdings>().Returns(holdings.ToList<Holdings>);
            //_session.Query<Holdings>().Returns(holdings.AsQueryable());
            _session.Query<Holdings>().Returns(holdings.AsQueryable());

            bool resultHoldings = _sut.HoldingExists(AccountId, Symbol);
            Assert.That(resultHoldings, Is.EqualTo(true));

        }

        [Test]
        public void NewHolding_ShouldCreateHoldingAsync()
        {
            int AccountId = 2;
            string CompanyName = "General Electric Co.";
            string Symbol = "GE";
            int StockCount = 5;
            float LatestCostPerStock = 10.65F;
            float Change = -0.33F; 
            float ChangePercentage = -0.02957F;
            var LastUpdated = DateTime.Now;

            _sut.NewHolding(AccountId, CompanyName, Symbol, StockCount,
                LatestCostPerStock, Change, ChangePercentage, LastUpdated);
            _session.Received(1).SaveAsync(Arg.Any<Holdings>());

        }

        [Test]
        public void UpdateHolding_ShouldUpdateHoldingAsync()
        {
            int AccountId = 2;
            string CompanyName = "General Electric Co.";
            string Symbol = "GE";
            int StockCount = 5;
            float LatestCostPerStock = 10.65F;
            float Change = -0.33F;
            float ChangePercentage = -0.02957F;
            var LastUpdated = DateTime.Now;

            _sut.UpdateHolding(AccountId, Symbol, StockCount, LatestCostPerStock, LastUpdated);

            var query = _session.CreateQuery("Update Holdings set stock_count =:StockCount, " +
                    "latest_cost_per_stock =:LatestCostPerStock, last_Updated =:LastUpdated " +
                    "where account_id =:AccountId and symbol =:Symbol");

            query.SetParameter("StockCount", StockCount);
            query.SetParameter("LatestCostPerStock", LatestCostPerStock);
            query.SetParameter("LastUpdated", LastUpdated);
            query.SetParameter("AccountId", AccountId);
            query.SetParameter("Symbol", Symbol);

            var resultUpdate = query.Received(1).ExecuteUpdateAsync();
            Assert.That(resultUpdate, Is.EqualTo(1));
        }


        /*
          [Test]
        public void NewAccount_ShouldCreateAccountAsync()
        {
            var UserId = 3;

            _sut.NewAccount(UserId);
            _session.Received(1).SaveAsync(Arg.Any<Account>());
            //_session.Received(1).SaveAsync(Arg.Is<Account>(a => a.Id == 0 && a.UserId == 3 && a.Balance == 100000));


        }
         
         */
    }
}