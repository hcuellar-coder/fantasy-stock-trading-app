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
            try
            {
                return _userService.GetUser(Email).Select(user => new UserModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName
                });
            }
            catch
            {
                throw new Exception("error getting user");
            }
        }

        [HttpGet("is_valid_user")]
        public bool isValidUser(string Email, string Password)
        {
            return _userService.isValidUser(Email, Password);
        }

        [HttpGet("check_user")]
        public IQueryable<UserModel> CheckUser(string Email)
        {
            try
            {
                return _userService.GetUser(Email).Select(user => new UserModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName
                });
            }
            catch
            {
                throw new Exception("error checking user");
            }
        }

        [HttpPost("new_user")]
        public async Task NewUser(JObject data)
        {
            try
            {
                var Email = data["Email"].ToString();
                var Password = data["Password"].ToString();
                var FirstName = data["FirstName"].ToString();
                var LastName = data["LastName"].ToString();

                await _userService.NewUser(Email, Password, FirstName, LastName);
            }
            catch
            {
                throw new Exception("error adding new user");
            }
        }

        [HttpGet("get_account")]
        public IQueryable<AccountModel> GetAccount(int UserId)
        {
            try
            {
                return _accountService.GetAccount(UserId)
                       .Select(account => new AccountModel
                       {
                           Id = account.Id,
                           UserId = account.UserId,
                           Balance = account.Balance,
                           PortfolioBalance = account.PortfolioBalance
                       });
            }
            catch
            {
                throw new Exception("error getting account");
            }
        }

        [HttpPost("new_account")]
        public async Task NewAccount(JObject data)
        {
            try
            {
                var UserId = Int32.Parse(data["UserId"].ToString());
                await _accountService.NewAccount(UserId);
            }
            catch
            {
                throw new Exception("error setting new account");
            }
        }

        [HttpPost("update_account")]
        public async Task UpdateAccount(JObject data)
        {
            try
            {
                var AccountId = Int32.Parse(data["AccountId"].ToString());
                var Balance = float.Parse(data["Balance"].ToString());
                var PortfolioBalance = float.Parse(data["PortfolioBalance"].ToString());

                await _accountService.UpdateAccount(AccountId, Balance, PortfolioBalance);
            }
            catch
            {
                throw new Exception("error updating account");
            }
        }

        [HttpGet("get_holdings")]
        public IQueryable<HoldingsModel> GetHoldings(int AccountId)
        {
            try
            {
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
            catch
            {
                throw new Exception("error getting holdings");
            }
        }

        [HttpPost("update_holding")]
        public async Task UpdateHolding(JObject data)
        {
            try
            {
                var AccountId = Int32.Parse(data["AccountId"].ToString());
                var CompanyName = data["CompanyName"].ToString();
                var Symbol = data["Symbol"].ToString();
                var StockCount = Int32.Parse(data["StockCount"].ToString());
                var LatestCostPerStock = float.Parse(data["LatestCostPerStock"].ToString());
                var Change = float.Parse(data["Change"].ToString());
                var ChangePercentage = float.Parse(data["ChangePercentage"].ToString());
                var LastUpdated = DateTime.Now;

                if (_holdingsService.HoldingExists(AccountId, Symbol))
                {
                    if (StockCount == 0)
                    {
                        await _holdingsService.DeleteHolding(AccountId, Symbol);
                    }
                    else
                    {
                        await _holdingsService.UpdateHolding(AccountId, Symbol,
                                        StockCount, LatestCostPerStock, LastUpdated);
                    }
                }
                else
                {
                    await _holdingsService.NewHolding(AccountId, CompanyName, Symbol, StockCount,
                                        LatestCostPerStock, Change, ChangePercentage, LastUpdated);
                }
            }
            catch
            {
                throw new Exception("error updating holding");
            }           
        }

        [HttpPost("update_holdings")]
        public async Task UpdateHoldings(JObject Holdings)
        {
            try
            {
                await _holdingsService.UpdateHoldings(Holdings);
            }
            catch
            {
                throw new Exception("error update holdings");
            }
        }

        [HttpPost("new_transaction")]
        public async Task NewTransaction(JObject data)
        {
            try
            {
                var AccountId = Int32.Parse(data["AccountId"].ToString());
                var Type = data["Type"].ToString();
                var Symbol = data["Symbol"].ToString();
                var StockCount = Int32.Parse(data["StockCount"].ToString());
                var CostPerStock = float.Parse(data["CostPerStock"].ToString());
                var CostPerTransaction = float.Parse(data["CostPerTransaction"].ToString());
                var TransactionDate = DateTime.Now;

                await _transactionService.NewTransaction(AccountId, Type, Symbol, StockCount,
                                        CostPerStock, CostPerTransaction, TransactionDate);
            }
            catch
            {
                throw new Exception("error setting new transaction");
            }
        }
    }
}
