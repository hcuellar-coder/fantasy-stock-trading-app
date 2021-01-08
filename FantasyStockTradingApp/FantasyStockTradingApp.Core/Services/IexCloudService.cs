using FantasyStockTradingApp.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using System.IO;

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
        private readonly string _path;
        private readonly string _token;

        public IexCloudService(IHttpClientFactory clientFactory,
                IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            _clientFactory = clientFactory;
            _configuration = configuration;
            _path = Path.GetFullPath(ToString());

            if (_hostingEnvironment.EnvironmentName == "Development")
            {
                _token = _configuration["Token_Key:token"];
            }
            else
            {
                _token = Environment.GetEnvironmentVariable("TOKENKEY");
            }

        }

        public async Task<Quote> GetQuote(string symbol)
        {
                var requestUri = "stock/" + symbol + "/quote?token=" + _token;
                var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
                var client = _clientFactory.CreateClient("iexCloud");

                var response = await client.SendAsync(request);
                var responseStream = await response.Content.ReadAsStringAsync();
                
                return JsonConvert.DeserializeObject<Quote>(responseStream);
        }


        public async Task<List<Quote>> GetMostActive()
        {
            var requestUri = "stock/market/list/mostactive?token=" + _token;
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            var client = _clientFactory.CreateClient("iexCloud");

            var response = await client.SendAsync(request);

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
            var requestUri = "stock/"+symbol+ "/chart/1m?token=" + _token;

            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            var client = _clientFactory.CreateClient("iexCloud");

            var response = await client.SendAsync(request);
            var responseStream = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<History>>(responseStream);

        }
    }
}
