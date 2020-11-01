using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using FantasyStockTradingApp.Models;
using FantasyStockTradingApp.Services;
using Microsoft.AspNetCore.Mvc;


namespace FantasyStockTradingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FantasyStockTradingController : ControllerBase
    {

        private readonly IUserService _userService;

        public FantasyStockTradingController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/<FantasyStockTradingController>
        [HttpGet("login")]
        public IQueryable<User> GetUser(string email, string password)
        {
            Console.WriteLine("in Login");
            Console.WriteLine("email = " + email);
            Console.WriteLine("password = " + password);
            var tempuser = new User();

            return _userService.GetUserInformation(email, password);
        }

        [HttpPost("new_user")]
        public async Task NewUser(string email, string password, string first_name, string last_name)
        {
            Console.WriteLine("in Post");
            Console.WriteLine("email = " + email);
            Console.WriteLine("password = " + password);
            Console.WriteLine("first_name = "+ first_name);
            Console.WriteLine("last_name = "+ last_name);
            //_userService.AddNewUser(email, password, first_name, last_name);
          await _userService.AddNewUser("heriberto.cuellar1329@gmail.com", "testing123", "Heriberto", "Cuellar");
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
