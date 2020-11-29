using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using FantasyStockTradingApp.Models;
using FantasyStockTradingApp.Core.Services;
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

        public FantasyStockTradingController(IUserService userService,
            ITransactionService transactionService, IAccountService accountService, 
            IHoldingsService holdingsService)
        {
            _userService = userService;
            _transactionService = transactionService;
            _accountService = accountService;
            _holdingsService = holdingsService;
        }

        [HttpGet("get_user")]
        public IQueryable<UserModel> GetUser(string Email)
        {
            Console.WriteLine("in get_user");
            Console.WriteLine("email = " + Email);

            return _userService.GetUser(Email).Select(user => new UserModel 
            { 
                Id = user.Id, 
                Email = user.Email, 
                FirstName = user.FirstName, 
                LastName = user.LastName 
            });
        }

        [HttpGet("is_valid_user")]
        public bool isValidUser(string Email, string Password)
        {
            Console.WriteLine("in check_user");
            Console.WriteLine("email = " + Email);
            Console.WriteLine("password = " + Password);

            return _userService.isValidUser(Email, Password);
        }

        [HttpGet("check_user")]
        public IQueryable<UserModel> CheckUser(string Email)
        {
            Console.WriteLine("in check_user");
            Console.WriteLine("email = " + Email);

            return _userService.GetUser(Email).Select(user => new UserModel 
            { 
                Id = user.Id, 
                Email = user.Email, 
                FirstName = user.FirstName, 
                LastName = user.LastName 
            });
        }

        [HttpPost("new_user")]
        public async Task NewUser(JObject data)
        {
            var Email = data["Email"].ToString();
            var Password = data["Password"].ToString();
            var FirstName = data["FirstName"].ToString();
            var LastName = data["LastName"].ToString();
            Console.WriteLine("in new_user");
            Console.WriteLine("mail = " + Email);
            Console.WriteLine("password = " + Password);
            Console.WriteLine("first_name = " + FirstName);
            Console.WriteLine("last_name = " + LastName);

            await _userService.NewUser(Email, Password, FirstName, LastName);
        }

        [HttpGet("get_account")]
        public IQueryable<AccountModel> GetAccount(int UserId)
        {
            Console.WriteLine("in get_account");
            Console.WriteLine("user_id = " + UserId);
            return _accountService.GetAccount(UserId)
                .Select(account => new AccountModel 
                { 
                    Id = account.Id,
                    UserId = account.UserId, 
                    Balance = account.Balance, 
                    PortfolioBalance = account.PortfolioBalance 
                });
        }

        [HttpPost("new_account")]
        public async Task NewAccount(JObject data)
        {
            var UserId = Int32.Parse(data["UserId"].ToString());
            Console.WriteLine("in new_account");
            Console.WriteLine("UserId = " + UserId);
            
            await _accountService.NewAccount(UserId);
        }

        [HttpPost("update_account")]
        public async Task UpdateAccount(JObject data)
        {
            var AccountId = Int32.Parse(data["AccountId"].ToString());
            var Balance = float.Parse(data["Balance"].ToString());
            var PortfolioBalance = float.Parse(data["PortfolioBalance"].ToString());

            Console.WriteLine("in update_account");
            Console.WriteLine("AccountId = " + AccountId);
            Console.WriteLine("Balance = " + Balance);
            Console.WriteLine("PortfolioBalance = " + PortfolioBalance);

            await _accountService.UpdateAccount(AccountId, Balance, PortfolioBalance);
        }

        [HttpGet("get_holdings")]
        public IQueryable<HoldingsModel> GetHoldings(int AccountId)
        {
            Console.WriteLine("in get_holdings");
            Console.WriteLine("account_id = " + AccountId);
            return _holdingsService.GetHoldings(AccountId)
                .Select(holdings => new HoldingsModel
                {
                    Id = holdings.Id,
                    AccountId = holdings.AccountId,
                    CompanyName = holdings.CompanyName,
                    Symbol = holdings.Symbol,
                    StockCount = holdings.StockCount,
                    LatestCostPerStock = holdings.LatestCostPerStock,
                    Change = holdings.Change,
                    ChangePercentage = holdings.ChangePercentage,
                    LastUpdated = holdings.LastUpdated
                });
        }

        [HttpPost("update_holding")]
        public async Task UpdateHolding(JObject data)
        {
            var AccountId = Int32.Parse(data["AccountId"].ToString());
            var CompanyName = data["CompanyName"].ToString();
            var Symbol = data["Symbol"].ToString();
            var StockCount = Int32.Parse(data["StockCount"].ToString());
            var LatestCostPerStock = float.Parse(data["LatestCostPerStock"].ToString());
            var Change = float.Parse(data["Change"].ToString());
            var ChangePercentage = float.Parse(data["ChangePercentage"].ToString());
            var LastUpdated = DateTime.Now;

            Console.WriteLine("in update_holding");
            Console.WriteLine("AccountId = " + AccountId);
            Console.WriteLine("Symbol = " + Symbol);
            Console.WriteLine("stock_count = " + StockCount);
            Console.WriteLine("cost = " + LatestCostPerStock);
            Console.WriteLine("updated time = " + LastUpdated);
            
            if (_holdingsService.HoldingExists(AccountId, Symbol))
            {
                if (StockCount == 0)
                {
                    await _holdingsService.DeleteHolding(AccountId, Symbol);
                } else
                {
                    await _holdingsService.UpdateHolding(AccountId, Symbol,
                                    StockCount, LatestCostPerStock, LastUpdated);
                }
            } else
            {
                await _holdingsService.NewHolding(AccountId, CompanyName, Symbol, StockCount,
                                   LatestCostPerStock, Change, ChangePercentage, LastUpdated);
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
            var AccountId = Int32.Parse(data["AccountId"].ToString());
            var Type = data["Type"].ToString();
            var Symbol = data["Symbol"].ToString();
            var StockCount = Int32.Parse(data["StockCount"].ToString());
            var CostPerStock = float.Parse(data["CostPerStock"].ToString());
            var CostPerTransaction = float.Parse(data["CostPerTransaction"].ToString());
            var TransactionDate = DateTime.Now;

            Console.WriteLine("in new_transaction");
            Console.WriteLine("account_id = " + AccountId);
            Console.WriteLine("type = " + Type);
            Console.WriteLine("symbol = " + Symbol);
            Console.WriteLine("stock_count = " + StockCount);
            Console.WriteLine("cost = " + CostPerStock);
            Console.WriteLine("cost = " + CostPerTransaction);
            Console.WriteLine("transaction_date = " + TransactionDate);

            await _transactionService.NewTransaction(AccountId, Type, Symbol, StockCount,
                                    CostPerStock, CostPerTransaction, TransactionDate);
        }
    }
}
