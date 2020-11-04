using FantasyStockTradingApp.Models;
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

namespace FantasyStockTradingApp.Services
{

    public interface IIexCloudService
    {
        Task<Quote> GetQuote(string symbol);

    }
    public class IexCloudService : IIexCloudService
    {
        private readonly IHttpClientFactory _clientFactory;

        public IexCloudService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }


        public async Task<Quote> GetQuote(string symbol)
        {
            Console.WriteLine("symbol = " + symbol);

            /*var queryStrings = new Dictionary<string, string>()
            {
                {"symbol", symbol }
            };
*/
            var requestUri = "stock/"+symbol+"/quote/";

            Console.WriteLine("requestUri = " + requestUri);


            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            var client = _clientFactory.CreateClient("iexCloud");
            Console.WriteLine("request = " + request);
            Console.WriteLine("client = " + client);

            var response = await client.SendAsync(request);
            Console.WriteLine("response = " + response);

            if (response.IsSuccessStatusCode == false)
            {
                var errorString = $"There was an error getting quote data: {response.ReasonPhrase}";
                throw new Exception(errorString);
            }

            var responseStream = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Quote>(responseStream);

        }

    }
}
