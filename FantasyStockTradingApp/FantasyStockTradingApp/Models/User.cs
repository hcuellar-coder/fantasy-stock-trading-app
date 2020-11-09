using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FantasyStockTradingApp.Models
{
    public class User
    {
        public virtual int id { get; set; }
        public virtual string email { get; set; }
        public virtual string password { get; set; }
        public virtual string first_name { get; set; }
        public virtual string last_name { get; set; }
    }
}
