using FantasyStockTradingApp.Core.Services;
using FantasyStockTradingApp.Core.Handler;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using NSubstitute;
using System;
using System.Net.Http;

namespace FantasyStockTradingApp.Tests
{
    [TestFixture]
    class IexCloudServiceTests
    {
        private IHttpClientFactory _clientFactory;
        private IConfiguration _configuration;
        private IWebHostEnvironment _hostingEnvironment;
        private IIexCloudService _sut;

        [SetUp]
        public void Setup()
        {
            _clientFactory = Substitute.For<IHttpClientFactory>();
            _configuration = Substitute.For<IConfiguration>();
            _hostingEnvironment = Substitute.For<IWebHostEnvironment>();
            _sut = new IexCloudService(_clientFactory, _configuration, _hostingEnvironment);
        }

        [Test]
        public void GetQuote_ShouldReturnQuote()
        {
            var response = @"{
                    ""Symbol"": ""GE"",
                    ""CompanyName"": ""General Electric Co."",
                    ""LatestPrice"": 10.65,
                    ""Change"": -0.33,
                    ""ChangePercent"": -0.02957
                    }";

            string Symbol = "GE";
            var messageHandler = new MockHttpMessageHandler(response);
            var client = new HttpClient(messageHandler) { BaseAddress = new Uri("https://localhost") };

            _clientFactory.CreateClient(Arg.Any<string>()).Returns(client);

            var result = _sut.GetQuote(Symbol);
            Assert.AreEqual(result.Result.Symbol, "GE");
            Assert.AreEqual(result.Result.LatestPrice, 10.65F);
        }

        [Test]
        public void GetMostActive_ShouldReturnMostActive()
        {
            var response = @"[{
                    ""Symbol"": ""GE"",
                    ""CompanyName"": ""General Electric Co."",
                    ""LatestPrice"": 10.65,
                    ""Change"": -0.33,
                    ""ChangePercent"": -0.02957
                    },
                    {
                    ""Symbol"": ""AAPL"",
                    ""CompanyName"": ""Apple Inc"",
                    ""LatestPrice"": 130.92,
                    ""Change"": 4.32,
                    ""ChangePercent"": 0.0341}]";

            var messageHandler = new MockHttpMessageHandler(response);
            var client = new HttpClient(messageHandler) { BaseAddress = new Uri("https://localhost") };

            _clientFactory.CreateClient(Arg.Any<string>()).Returns(client);

            var result = _sut.GetMostActive();
            Assert.AreEqual(result.Result[0].Symbol, "GE");
            Assert.AreEqual(result.Result[1].Symbol, "AAPL");
        }

        [Test]
        public void GetHistory_ShouldReturnMostActive()
        {
            var response = @"[{
                    ""Date"": ""2020-12-08"",
                    ""Close"": 10.65},
                    {
                    ""Date"": ""2020-12-09"",
                    ""Close"": 10.95},
                    {
                    ""Date"": ""2020-12-10"",
                    ""Close"": 11},
                    {
                    ""Date"": ""2020-12-11"",
                    ""Close"": 10.75},
                    {
                    ""Date"": ""2020-12-12"",
                    ""Close"": 10.78}]";

            string Symbol = "GE";
            var messageHandler = new MockHttpMessageHandler(response);
            var client = new HttpClient(messageHandler) { BaseAddress = new Uri("https://localhost") };

            _clientFactory.CreateClient(Arg.Any<string>()).Returns(client);
            var result = _sut.GetHistory(Symbol);
            Assert.AreEqual(result.Result[0].Close, 10.65F);
            Assert.AreEqual(result.Result[1].Close, 10.95F);
            Assert.AreEqual(result.Result[2].Close, 11F);
            Assert.AreEqual(result.Result[3].Close, 10.75F);
            Assert.AreEqual(result.Result[4].Close, 10.78F);
        }
    }
}
