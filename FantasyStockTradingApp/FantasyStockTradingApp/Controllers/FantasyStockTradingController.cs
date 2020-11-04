﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using FantasyStockTradingApp.Models;
using FantasyStockTradingApp.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FantasyStockTradingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FantasyStockTradingController : ControllerBase
    {

        private readonly IUserService _userService;
        private readonly IIexCloudService _iexCloudService;
        private readonly ITransactionService _transactionService;

        //This is were the problem isf
        public FantasyStockTradingController(IUserService userService,
                                     IIexCloudService iexCloudService,
                                     ITransactionService transactionService)
        {
            _userService = userService;
            _iexCloudService = iexCloudService;
            _transactionService = transactionService;
        }

        // GET: api/<FantasyStockTradingController>
        [HttpGet("login")]
        public IQueryable<User> GetUser(string email, string password)
        {
            Console.WriteLine("in Login");
            Console.WriteLine("email = " + email);
            Console.WriteLine("password = " + password);

            return _userService.GetUserInformation(email, password);
        }

        // GET: api/<FantasyStockTradingController>
        [HttpGet("get_quote")]
        public Task<Quote> GetQuote(string symbol)
        {
            Console.WriteLine("getting Quote");
            Console.WriteLine("symbol = " + symbol);

            return _iexCloudService.GetQuote(symbol);
        }

        [HttpPost("new_user")]
        public async Task NewUser(JObject data)
        {
            var email = data["email"].ToString();
            var password = data["password"].ToString();
            var first_name = data["first_name"].ToString();
            var last_name = data["last_name"].ToString();
            Console.WriteLine("in Post");
            Console.WriteLine("email = " + email);
            Console.WriteLine("password = " + password);
            Console.WriteLine("first_name = " + first_name);
            Console.WriteLine("last_name = " + last_name);

            await _userService.AddNewUser(email, password, first_name, last_name);
        }

        [HttpPost("stock_transaction")]
        public async Task StockTransaction(JObject data)
        {
            var cost = float.Parse(data["cost"].ToString());
            var stock_count = Int32.Parse(data["stock_count"].ToString());
            var symbol = data["symbol"].ToString();
            var type = data["type"].ToString();

            Console.WriteLine("in Post");
            Console.WriteLine("cost = " + cost);
            Console.WriteLine("stock_count = " + stock_count);
            Console.WriteLine("symbol = " + symbol);
            Console.WriteLine("type = " + type);

            await _transactionService.StockTransaction(cost, stock_count, symbol, type);
        }

        // GET api/<FantasyStockTradingController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<FantasyStockTradingController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<FantasyStockTradingController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<FantasyStockTradingController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
