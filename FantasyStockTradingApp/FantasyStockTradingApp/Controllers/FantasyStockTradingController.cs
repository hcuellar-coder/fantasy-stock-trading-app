using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using FantasyStockTradingApp.Models;
using FantasyStockTradingApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FantasyStockTradingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FantasyStockTradingController : ControllerBase
    {

        private readonly IUserService _userService;
        private readonly ITransactionService _transactionService;
        private readonly IAccountService _accountService;
        private readonly IHoldingsService _holdingsService;
        private readonly IUserLoginService _userLoginService;

        public FantasyStockTradingController(IUserService userService,
            ITransactionService transactionService, IAccountService accountService, 
            IHoldingsService holdingsService, IUserLoginService userLoginService)
        {
            _userService = userService;
            _transactionService = transactionService;
            _accountService = accountService;
            _holdingsService = holdingsService;
            _userLoginService = userLoginService;
        }

        [HttpGet("get_user")]
        public IQueryable<User> GetUser(string email)
        {
            Console.WriteLine("in get_user");
            Console.WriteLine("email = " + email);

            return _userService.GetUser(email);
        }

        [HttpGet("is_valid_user")]
        public bool isValidUser(string email, string password)
        {
            Console.WriteLine("in check_user");
            Console.WriteLine("email = " + email);
            Console.WriteLine("password = " + password);

            return _userLoginService.isValidUser(email, password);
        }

        [HttpGet("check_user")]
        public IQueryable<User> CheckUser(string email)
        {
            Console.WriteLine("in check_user");
            Console.WriteLine("email = " + email);

            return _userService.GetUser(email);
        }

        [HttpPost("new_user")]
        public async Task<User> NewUser(JObject data)
        {
            var email = data["email"].ToString();
            var first_name = data["first_name"].ToString();
            var last_name = data["last_name"].ToString();
            Console.WriteLine("in new_user");
            Console.WriteLine("email = " + email);
            Console.WriteLine("first_name = " + first_name);
            Console.WriteLine("last_name = " + last_name);

            return await _userService.NewUser(email, first_name, last_name);
        }

        [HttpPost("new_user_login")]
        public async Task NewUserLogin(JObject data)
        {
            var email = data["email"].ToString();
            var password = data["password"].ToString();
            var user_id = Int32.Parse(data["user_id"].ToString());
            Console.WriteLine("in new_user");
            Console.WriteLine("email = " + email);
            Console.WriteLine("password = " + password);
            Console.WriteLine("user_id = " + user_id);

            await _userLoginService.NewUserLogin(email, password, user_id);
        }

        [HttpGet("get_account")]
        public IQueryable<Account> GetAccount(int user_id)
        {
            Console.WriteLine("in get_account");
            Console.WriteLine("user_id = " + user_id);
            return _accountService.GetAccount(user_id);
        }

        [HttpPost("new_account")]
        public async Task<Account> NewAccount(JObject data)
        {
            var user_id = Int32.Parse(data["user_id"].ToString());
            Console.WriteLine("in new_account");
            Console.WriteLine("user_id = " + user_id);

            return await _accountService.NewAccount(user_id);
        }

        [HttpPost("update_account")]
        public async Task UpdateAccount(JObject data)
        {
            var account_id = Int32.Parse(data["account_id"].ToString());
            var balance = float.Parse(data["balance"].ToString());
            var portfolio_balance = float.Parse(data["portfolio_balance"].ToString());

            Console.WriteLine("in update_account");
            Console.WriteLine("account_id = " + account_id);
            Console.WriteLine("balance = " + balance);
            Console.WriteLine("portfolio_balance = " + portfolio_balance);

            await _accountService.UpdateAccount(account_id, balance, portfolio_balance);
        }

        [HttpGet("get_holdings")]
        public IQueryable<Holdings> GetHoldings(int account_id)
        {
            Console.WriteLine("in get_holdings");
            Console.WriteLine("account_id = " + account_id);
            return _holdingsService.GetHoldings(account_id);
        }

        [HttpPost("update_holding")]
        public async Task UpdateHolding(JObject data)
        {
            var account_id = Int32.Parse(data["account_id"].ToString());
            var company_name = data["company_name"].ToString();
            var symbol = data["symbol"].ToString();
            var stock_count = Int32.Parse(data["stock_count"].ToString());
            var latest_cost_per_stock = float.Parse(data["latest_cost_per_stock"].ToString());
            var change = float.Parse(data["change"].ToString());
            var change_percentage = float.Parse(data["change_percentage"].ToString());
            var last_Updated = DateTime.Now;

            Console.WriteLine("in update_holding");
            Console.WriteLine("account_id = " + account_id);
            Console.WriteLine("symbol = " + symbol);
            Console.WriteLine("stock_count = " + stock_count);
            Console.WriteLine("cost = " + latest_cost_per_stock);
            Console.WriteLine("updated time = " + last_Updated);
            
            if (_holdingsService.holdingExists(account_id, symbol))
            {
                if (stock_count == 0)
                {
                    await _holdingsService.DeleteHolding(account_id, symbol);
                } else
                {
                    await _holdingsService.UpdateHolding(account_id, symbol,
                                    stock_count, latest_cost_per_stock, last_Updated);
                }
            } else
            {
                await _holdingsService.NewHolding(account_id, company_name, symbol, stock_count,
                                   latest_cost_per_stock, change, change_percentage, last_Updated);
            }
        }

        [HttpPost("update_holdings")]
        public async Task UpdateHoldings(JObject data)
        {
            Console.WriteLine("in update_holdings");
            Console.WriteLine("data = " + data);
            
            await _holdingsService.UpdateHoldings(data);
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
            var transaction_date = DateTime.Now;

            Console.WriteLine("in new_transaction");
            Console.WriteLine("account_id = " + account_id);
            Console.WriteLine("type = " + type);
            Console.WriteLine("symbol = " + symbol);
            Console.WriteLine("stock_count = " + stock_count);
            Console.WriteLine("cost = " + cost_per_stock);
            Console.WriteLine("cost = " + cost_per_transaction);
            Console.WriteLine("transaction_date = " + transaction_date);

            await _transactionService.NewTransaction(account_id, type, symbol, stock_count,
                                    cost_per_stock, cost_per_transaction, transaction_date);
        }
    }
}
