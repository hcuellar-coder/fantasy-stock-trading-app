using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FantasyStockTradingApp.Models
{
    public class UserLogin
    {
        public virtual int id { get; set; }
        public virtual string email { get; set; }
        public virtual string password { get; set; }
        public virtual int user_account_id { get; set; }
    }
}
