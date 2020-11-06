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
        private readonly IAccountService _accountService;
        private readonly IHoldingsService _holdingsService;

        //This is were the problem isf
        public FantasyStockTradingController(IUserService userService,
            IIexCloudService iexCloudService, ITransactionService transactionService,
            IAccountService accountService, IHoldingsService holdingsService)
        {
            _userService = userService;
            _iexCloudService = iexCloudService;
            _transactionService = transactionService;
            _accountService = accountService;
            _holdingsService = holdingsService;
        }

        // GET: api/<FantasyStockTradingController>
        [HttpGet("login")]
        public IQueryable<User> GetUser(string email, string password)
        {
            Console.WriteLine("in Login");
            Console.WriteLine("email = " + email);
            Console.WriteLine("password = " + password);

            return _userService.GetUser(email, password);
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

            await _userService.NewUser(email, password, first_name, last_name);
        }

        [HttpPost("new_account")]
        public async Task NewAccount(JObject data)
        {
            var user_id = Int32.Parse(data["user_id"].ToString());
            Console.WriteLine("in Post");
            Console.WriteLine("user_id = " + user_id);

            await _accountService.NewAccount(user_id);
        }

        [HttpPost("new_transaction")]
        public async Task NewTransaction(JObject data)
        {
            var account_id = Int32.Parse(data["account_id"].ToString());
            var type = data["type"].ToString();
            var symbol = data["symbol"].ToString();
            var stock_count = Int32.Parse(data["stock_count"].ToString());
            var cost_per_stock = float.Parse(data["cost_per_stock"].ToString());
            var cost_per_transaction = float.Parse(data["cost_per_transaction"].ToString());

            Console.WriteLine("in Post");
            Console.WriteLine("account_id = " + account_id);
            Console.WriteLine("type = " + type);
            Console.WriteLine("symbol = " + symbol);
            Console.WriteLine("stock_count = " + stock_count);
            Console.WriteLine("cost = " + cost_per_stock);
            Console.WriteLine("cost = " + cost_per_transaction);

            //1. Inserting into transaction table
            await _transactionService.StockTransaction(account_id, type, symbol, 
                                    stock_count, cost_per_stock, cost_per_transaction);
        }


        [HttpPost("update_holdings")]
        public async Task UpdateHoldings(JObject data)
        {
            var account_id = Int32.Parse(data["account_id"].ToString());
            var symbol = data["symbol"].ToString();
            var stock_count = Int32.Parse(data["stock_count"].ToString());
            var latest_cost_per_stock = float.Parse(data["latest_cost_per_stock"].ToString());
            var last_Updated = data["updated_time"].ToString();

            Console.WriteLine("in Post");
            Console.WriteLine("account_id = " + account_id);
            Console.WriteLine("symbol = " + symbol);
            Console.WriteLine("stock_count = " + stock_count);
            Console.WriteLine("cost = " + latest_cost_per_stock);
            Console.WriteLine("updated time = " + last_Updated);

            //1. Updating Holdings
            await _holdingsService.UpdateHoldings(account_id, symbol,
                                    stock_count, latest_cost_per_stock, last_Updated);
        }

        [HttpPost("update_account")]
        public async Task UpdateAccount(JObject data)
        {
            var account_id = Int32.Parse(data["account_id"].ToString());
            var type = data["type"].ToString();
            var symbol = data["symbol"].ToString();
            var stock_count = Int32.Parse(data["stock_count"].ToString());
            var cost_per_stock = float.Parse(data["cost_per_stock"].ToString());
            var cost_per_transaction = float.Parse(data["cost_per_transaction"].ToString());

            Console.WriteLine("in Post");
            Console.WriteLine("account_id = " + account_id);
            Console.WriteLine("type = " + type);
            Console.WriteLine("symbol = " + symbol);
            Console.WriteLine("stock_count = " + stock_count);
            Console.WriteLine("cost = " + cost_per_stock);
            Console.WriteLine("cost = " + cost_per_transaction);

            //1. Update Account Information
            await _transactionService.StockTransaction(account_id, type, symbol,
                                    stock_count, cost_per_stock, cost_per_transaction);
        }



        /*// GET api/<FantasyStockTradingController>/5
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
        }*/
    }
}
