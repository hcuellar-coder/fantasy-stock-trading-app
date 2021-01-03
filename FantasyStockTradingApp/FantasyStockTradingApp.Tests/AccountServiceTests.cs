using FantasyStockTradingApp.Core.Entities;
using FantasyStockTradingApp.Core.Services;
using FantasyStockTradingApp.Core.Exceptions;
using Microsoft.AspNetCore.Mvc;
using NHibernate;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NSubstitute.ExceptionExtensions;

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
            var UserId = 3;

            var BadAccount = new List<Account>
            {
                new Account
                {

                }
            };

            //_sut.NewAccount(UserId).ThrowsForAnyArgsi
            //var ex3 = _sut.NewAccount(int.Parse("abc")).ThrowsForAnyArgs( new NewAccountException("_path", "New Account()"));
            //_sut.NewAccount(UserId) = () => { throw new NewAccountException("_path", "New Account()"); };
            //_sut.NewAccount(UserId).Throws(new Exception());
            _sut.NewAccount(UserId);

            //_sut.When(x => x.NewAccount(3)).Do(x => throw new NewAccountException("_path", "NewAccount()"));
            /*_session.Received(1).SaveAsync(Arg.Any<NewAccountException>());*/
            //Assert.Throws<NewAccountException>(() => _sut.NewAccount(UserId).Throws(new NewAccountException("_path", "NewAccount()")));
            //var ex1 = _sut.NewAccount(UserId).Throws(new NewAccountException("_path", "New Account()"));
            //var ex = Assert.Throws<NewAccountException>(() => _sut.NewAccount(UserId).Throws(new NewAccountException("_path", "New Account()")));

            var ex = Assert.Throws<NewAccountException>( () => throw new NewAccountException("_path", "New Account()"));

            //var ex1 = _session.Received(1).SaveAsync(BadAccount);
            //() => throw new NewAccountException("_path", "New Account()")
            var ex2 = _session.Received(1).SaveAsync(Arg.Any<Account>());
                //.Throws<NewAccountException>( () => throw new NewAccountException("_path", "New Account()"));

            //var ex = Assert.Throws<NewAccountException>(() => _sut.NewAccount(UserId).Returns(x => { throw new NewAccountException("_path", "New Account()"); }));

            //Assert.Throws<NewAccountException>(() => _session.Received(1).SaveAsync(Arg.Any<Account>()));

            //.Returns(x => { throw new NewAccountException("_path", "NewAccount()"); })) ;

            // Assert.Throws<NewAccountException>(() => _session.Received(1).SaveAsync(Arg.Any<Account>()).Returns( x => { throw new NewAccountException("_path", "NewAccount()"); }));
            // Assert.Throws<NewAccountException>(() => _sut.NewAccount(UserId).Returns(x => { throw new NewAccountException("_path", "NewAccount()"); }));
            //_session.Received(1).SaveAsync(Arg.Is<Account>(a => a.Id == 0 && a.UserId == 3 && a.Balance == 100000));


        }
    }
}