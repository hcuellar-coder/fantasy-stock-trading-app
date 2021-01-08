using FantasyStockTradingApp.Core.Entities;
using FantasyStockTradingApp.Core.Services;
using FantasyStockTradingApp.Core.Exceptions;
using NHibernate;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using NSubstitute.ExceptionExtensions;
using Newtonsoft.Json.Linq;

namespace FantasyStockTradingApp.Tests
{
    public class TestHolding
    {
        public virtual int accountId { get; set; }
        public virtual string symbol { get; set; }
        public virtual int stockCount { get; set; }
    }

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
        public void GetHoldings_ShouldThrowException()
        {
            var AccountId = 2;
            _session.BeginTransaction().ThrowsForAnyArgs(new Exception());
            Assert.Throws<GetHoldingException>(() => _sut.GetHoldings(AccountId));
        }

        [Test]
        public void HoldingExists_ShouldReturnTrue()
        {
            int AccountId = 2;
            string Symbol = "GE";
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
                    Symbol = Symbol,
                    StockCount = 5,
                    LatestCostPerStock = LatestCostPerStock,
                    Change = Change,
                    ChangePercentage = ChangePercentage,
                    LastUpdated = LastUpdated
                }
            };

            _session.Query<Holdings>().Returns(holdings.AsQueryable());

            bool resultHoldings = _sut.HoldingExists(AccountId, Symbol);
            Assert.That(resultHoldings, Is.EqualTo(true));

        }

        [Test]
        public void HoldingExists_ShouldThrowException()
        {
            int AccountId = 2;
            string Symbol = "GE";

            _session.BeginTransaction().ThrowsForAnyArgs(new Exception());
            Assert.Throws<HoldingExistsException>(() => _sut.HoldingExists(AccountId, Symbol));
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
        public void NewHolding_ShouldThrowException()
        {
            int AccountId = 2;
            string CompanyName = "General Electric Co.";
            string Symbol = "GE";
            int StockCount = 5;
            float LatestCostPerStock = 10.65F;
            float Change = -0.33F;
            float ChangePercentage = -0.02957F;
            var LastUpdated = DateTime.Now;

            _session.BeginTransaction().ThrowsForAnyArgs(new Exception());
            Assert.ThrowsAsync<NewHoldingException>(() => _sut.NewHolding(AccountId, CompanyName, Symbol, StockCount,
                                                            LatestCostPerStock, Change, ChangePercentage, LastUpdated));
        }

        [Test]
        public void UpdateHolding_ShouldUpdateHoldingAsync()
        {
            int AccountId = 2;
            string CompanyName = "General Electric Co.";
            string Symbol = "GE";
            int StockCount = 10;
            float LatestCostPerStock = 10.95F;
            float Change = -0.33F;
            float ChangePercentage = -0.02957F;
            var LastUpdated = DateTime.Now;

            var Holding = new List<Holdings>
            {
                new Holdings
                {
                    Id = 2,
                    AccountId = AccountId,
                    CompanyName = CompanyName,
                    Symbol = Symbol,
                    StockCount = 5,
                    LatestCostPerStock = 10.65F,
                    Change = Change,
                    ChangePercentage = ChangePercentage,
                    LastUpdated = LastUpdated
                }
            };

            _session.Query<Holdings>().Returns(Holding.AsQueryable());
            _sut.UpdateHolding(AccountId, Symbol, StockCount, LatestCostPerStock, LastUpdated);
            Assert.That(Holding[0].StockCount == StockCount && Holding[0].LatestCostPerStock == LatestCostPerStock);
        }

        [Test]
        public void UpdateHolding_ShouldThrowException()
        {
            int AccountId = 2;
            string Symbol = "GE";
            int StockCount = 5;
            float LatestCostPerStock = 10.65F;
            var LastUpdated = DateTime.Now;

            _session.BeginTransaction().ThrowsForAnyArgs(new Exception());
            Assert.ThrowsAsync<UpdateHoldingException>(() => _sut.UpdateHolding(AccountId, Symbol, StockCount, LatestCostPerStock, LastUpdated));
        }

        [Test]
        public void UpdateHoldings_ShouldUpdateHoldingAsync()
        {
            
            int AccountId = 2;
            string Symbol = "GE";
            int StockCount = 10;

            var Holdings = new List<TestHolding>
            {
                new TestHolding
                {
                    accountId = AccountId,
                    symbol = Symbol,
                    stockCount = StockCount
                }
            };

            JObject Holding = new JObject();
            Holding["Holdings"] = JToken.FromObject(Holdings);

            _sut.UpdateHoldings(Holding);

        }

        [Test]
        public void UpdateHoldings_ShouldThrowException()
        {
            int AccountId = 2;
            string CompanyName = "General Electric Co.";
            string Symbol = "GE";
            int StockCount = 5;
            float LatestCostPerStock = 10.65F;
            float Change = -0.33F;
            float ChangePercentage = -0.02957F;
            var LastUpdated = DateTime.Now;

            var holdings = new Holdings
                {
                    Id = 2,
                    AccountId = AccountId,
                    CompanyName = CompanyName,
                    Symbol = Symbol,
                    StockCount = StockCount,
                    LatestCostPerStock = LatestCostPerStock,
                    Change = Change,
                    ChangePercentage = ChangePercentage,
                    LastUpdated = LastUpdated
            };

            _session.BeginTransaction().ThrowsForAnyArgs(new Exception());
            Assert.ThrowsAsync<UpdateHoldingsException>(() => _sut.UpdateHoldings(JObject.FromObject(holdings)));
        }


        [Test]
        public void DeleteHolding_ShouldDeleteHoldingAsync()
        {
            int AccountId = 2;
            string Symbol = "GE";

            _sut.DeleteHolding(AccountId, Symbol);
            _session.DeleteAsync(Arg.Any<Holdings>());

        }


        [Test]
        public void DeleteHolding_ShouldThrowException()
        {
            int AccountId = 2;
            string Symbol = "GE";

            _session.BeginTransaction().ThrowsForAnyArgs(new Exception());
            Assert.ThrowsAsync<DeleteHoldingsException>(() => _sut.DeleteHolding(AccountId, Symbol));
        }


    }
}