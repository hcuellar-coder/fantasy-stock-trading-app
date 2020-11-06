using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FantasyStockTradingApp.Models
{
    public class Account
    {
        public virtual int Id { get; set; }
        public virtual int User_Id { get; set; }
        public virtual float Balance { get; set; }
        public virtual float Portfolio_Balance { get; set; }
    }
}
