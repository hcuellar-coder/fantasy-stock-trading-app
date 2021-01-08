using FantasyStockTradingApp.Core.Entities;
using FantasyStockTradingApp.Core.Services;
using FantasyStockTradingApp.Core.Handler;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using System.Net;

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

        /*[Test]
        public async void GetQuote_ShouldReturnQuote()
        {
            
            string Symbol = "GE";
            string CompanyName = "General Electric Co.";
            float LatestPrice = 10.65F;
            float Change = -0.33F;
            float ChangePercent = -0.02957F;

            var Quote = new List<Quote>
            {
                new Quote
                {
                    Symbol = Symbol,
                    CompanyName = CompanyName,
                    LatestPrice = LatestPrice,
                    Change = Change,
                    ChangePercent = ChangePercent
                }
            };


            var response = @"{
                    ""Symbol"": ""GE"",
                    ""CompanyName"": ""General Electric Co."",
                    ""LatestPrice"": 10.65F,
                    ""Change"": -0.33F,
                    ""ChangePercent"": -0.02957F
                    }";

            var messageHandler = new MockHttpMessageHandler(response, HttpStatusCode.OK);
            var httpClient = new HttpClient(messageHandler);
            
            var responseStream = 

            var result = _sut.GetQuote(Symbol);



            Assert.IsNotNull(result);
        }*/

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
            var result = _sut.GetMostActive();
            Assert.IsNotNull(result);
        }

        [Test]
        public void GetHistory_ShouldReturnMostActive()
        {
            string Symbol = "GE";
            var result = _sut.GetHistory(Symbol);
            Assert.IsNotNull(result);
        }
    }
}
