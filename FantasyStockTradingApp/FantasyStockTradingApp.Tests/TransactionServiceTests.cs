﻿using FantasyStockTradingApp.Core.Entities;
using FantasyStockTradingApp.Core.Services;
using NHibernate;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace FantasyStockTradingApp.Tests
{
    [TestFixture]
    class TransactionServiceTests
    {
        private INHibernateService _nHibernateService;
        private ISession _session;
        private ITransactionService _sut;

        [SetUp]
        public void Setup()
        {
            _nHibernateService = Substitute.For<INHibernateService>();
            _session = Substitute.For<ISession>();
            _nHibernateService.OpenSession().Returns(_session);
            _sut = new TransactionService(_nHibernateService);
        }

        [Test]
        public void NewTransaction_ShouldCreateTransactionAsync()
        {
            int AccountId = 2;
            string Type = "buy";
            string Symbol = "GE";
            int StockCount = 10;
            float CostPerStock = 10.83F;
            float CostPerTransaction = 108.3F;
            var TransactionDate = DateTime.Now;


            _sut.NewTransaction(AccountId,Type,Symbol,StockCount,CostPerStock,CostPerTransaction,TransactionDate);
            _session.Received(1).SaveAsync(Arg.Any<Transaction>());
            //_session.Received(1).SaveAsync(Arg.Is<Account>(a => a.Id == 0 && a.UserId == 3 && a.Balance == 100000));


        }
    }
}
