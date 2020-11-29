using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FantasyStockTradingApp.Models
{
    public class UserModel
    {
        public virtual int Id { get; set; }
        public virtual string Email { get; set; }
        public virtual string Password { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
    }
}
