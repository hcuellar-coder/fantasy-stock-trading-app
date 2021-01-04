using FantasyStockTradingApp.Core.Entities;
using FantasyStockTradingApp.Core.Exceptions;
using FantasyStockTradingApp.Core.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace FantasyStockTradingApp.Tests
{
    [TestFixture]
    class IexCloudServiceTests
    {
        private IHttpClientFactory _clientFactory;
        private IConfiguration _configuration;
        private IWebHostEnvironment _hostingEnvironment;
        private IIexCloudService _sut;
        private string _token;

        [SetUp]
        public void Setup()
        {
            _clientFactory = Substitute.For<IHttpClientFactory>();
            _configuration = Substitute.For<IConfiguration>();
            _hostingEnvironment = Substitute.For<IWebHostEnvironment>();
            _sut = new IexCloudService(_clientFactory, _configuration, _hostingEnvironment);

            if (_hostingEnvironment.EnvironmentName == "Development")
            {
                _token = _configuration["Token_Key:token"];
            }
            else
            {
                _token = Environment.GetEnvironmentVariable("TOKENKEY");
            }
        }

        [Test]
        public void GetQuote_ShouldReturnQuote()
        {
            string Symbol = "GE";
            var result = _sut.GetQuote(Symbol);
            Assert.IsNotNull(result);
        }

    }
}
