using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FantasyStockTradingApp.Core.Entities
{
    public class Account
    {
        public virtual int Id { get; set; }
        public virtual int UserId { get; set; }
        public virtual float Balance { get; set; }
        public virtual float PortfolioBalance { get; set; }
    }
}
