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
using NSubstitute.ExceptionExtensions;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

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
        public async Task UpdateHolding_ShouldUpdateHoldingAsync()
        {
            int AccountId = 2;
            string CompanyName = "General Electric Co.";
            string Symbol = "GE";
            int StockCount = 5;
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
                    CompanyName = CompanyName,
                    Symbol = Symbol,
                    StockCount = StockCount,
                    LatestCostPerStock = LatestCostPerStock,
                    Change = Change,
                    ChangePercentage = ChangePercentage,
                    LastUpdated = LastUpdated
                }
            };


            //_session.Query<Holdings>().Returns(holdings.AsQueryable());

            await _sut.UpdateHolding(AccountId, Symbol, StockCount, LatestCostPerStock, LastUpdated);
            
            /*_session.Received(1).SaveAsync(Arg.Any<Holdings>());*/

             var query = _session.CreateQuery("Update Holdings set stock_count =:StockCount, " +
                     "latest_cost_per_stock =:LatestCostPerStock, last_Updated =:LastUpdated " +
                     "where account_id =:AccountId and symbol =:Symbol");

             query.SetParameter("StockCount", StockCount);
             query.SetParameter("LatestCostPerStock", 10.55F);
             query.SetParameter("LastUpdated", LastUpdated);
             query.SetParameter("AccountId", AccountId);
             query.SetParameter("Symbol", Symbol);

             var resultUpdate = await query.Received(1).ExecuteUpdateAsync();
             Assert.That(resultUpdate, Is.EqualTo(1));
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
            string CompanyName = "General Electric Co.";
            string Symbol = "GE";
            int StockCount = 5;
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

           /* _sut.UpdateHoldings(holdings);

            var query = _session.CreateQuery("Update Holdings set stock_count =:StockCount, " +
                    "latest_cost_per_stock =:LatestCostPerStock, last_Updated =:LastUpdated " +
                    "where account_id =:AccountId and symbol =:Symbol");

            query.SetParameter("StockCount", StockCount);
            query.SetParameter("LatestCostPerStock", LatestCostPerStock);
            query.SetParameter("LastUpdated", LastUpdated);
            query.SetParameter("AccountId", AccountId);
            query.SetParameter("Symbol", Symbol);

            var resultUpdate = query.Received(1).ExecuteUpdateAsync();
            Assert.That(resultUpdate, Is.EqualTo(1));*/
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
            string CompanyName = "General Electric Co.";
            string Symbol = "GE";
            int StockCount = 5;
            float LatestCostPerStock = 10.65F;
            float Change = -0.33F;
            float ChangePercentage = -0.02957F;
            var LastUpdated = DateTime.Now;

            _sut.DeleteHolding(AccountId, Symbol);

            var query = _session.CreateQuery("Delete from Holdings " +
                        "where account_id =:AccountId and symbol =:Symbol");
            query.SetParameter("AccountId", AccountId);
            query.SetParameter("Symbol", Symbol);

            var resultDelete = query.Received(1).ExecuteUpdateAsync();
            Assert.That(resultDelete, Is.EqualTo(1));
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