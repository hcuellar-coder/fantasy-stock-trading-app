﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


namespace FantasyStockTradingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FantasyStockTradingController : ControllerBase
    {
        // GET: api/<FantasyStockTradingController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
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
