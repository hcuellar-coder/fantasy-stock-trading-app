using FantasyStockTradingApp.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Web;
using Microsoft.AspNetCore.Server.HttpSys;
using System.Net;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;

namespace FantasyStockTradingApp.Core.Services
{

    public interface IIexCloudService
    {
        Task<Quote> GetQuote(string symbol);
        Task<List<Quote>> GetMostActive();
        Task<List<History>> GetHistory(string symbol);

    }
    public class IexCloudService : IIexCloudService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public IexCloudService(IHttpClientFactory clientFactory,
                IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            _clientFactory = clientFactory;
            _configuration = configuration;

        }

        public async Task<Quote> GetQuote(string symbol)
        {
            try
            {
                var token = "";
                if (_hostingEnvironment.EnvironmentName == "Development")
                {
                    token = _configuration["Token_Key:token"];
                }
                else
                {
                    token = Environment.GetEnvironmentVariable("TOKENKEY");
                }

                var requestUri = "stock/" + symbol + "/quote?token=" + token;
                var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
                var client = _clientFactory.CreateClient("iexCloud");

                var response = await client.SendAsync(request);
                var responseStream = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<Quote>(responseStream);

            }
            catch (Exception)
            {

                throw;
            }
            
        }


        public async Task<List<Quote>> GetMostActive()
        {
            var token = "";
            if (_hostingEnvironment.EnvironmentName == "Development")
            {
                token = _configuration["Token_Key:token"];
            }
            else
            {
                token = Environment.GetEnvironmentVariable("TOKENKEY");
            }

            var requestUri = "stock/market/list/mostactive?token=" + token;
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            var client = _clientFactory.CreateClient("iexCloud");

            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode == false)
            {
                var errorString = $"There was an error getting quote data: {response.ReasonPhrase}";
                throw new Exception(errorString);
            }

            var responseStream = await response.Content.ReadAsStringAsync();
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            return JsonConvert.DeserializeObject<List<Quote>>(responseStream, settings);

        }

        public async Task<List<History>> GetHistory(string symbol)
        {
            var token = "";
            if (_hostingEnvironment.EnvironmentName == "Development")
            {
                token = _configuration["Token_Key:token"];
            }
            else
            {
                token = Environment.GetEnvironmentVariable("TOKENKEY");
            }

            var requestUri = "stock/"+symbol+ "/chart/1m?token=" + token;

            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            var client = _clientFactory.CreateClient("iexCloud");

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode == false)
            {
                var errorString = $"There was an error getting quote data: {response.ReasonPhrase}";
                throw new Exception(errorString);
            }

            var responseStream = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<History>>(responseStream);

        }
    }
}
