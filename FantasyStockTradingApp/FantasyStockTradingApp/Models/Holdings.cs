using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FantasyStockTradingApp.Models
{
    public class Holdings
    {
        public virtual int Id { get; set; }
        public virtual int Account_Id { get; set; }
        public virtual string Company_Name { get; set; }
        public virtual string Symbol { get; set; }
        public virtual int Stock_Count { get; set; }
        public virtual float Latest_Cost_per_Stock { get; set; }
        public virtual float Change { get; set; }
        public virtual float Change_Percentage {get; set;}
        public virtual DateTime Last_Updated { get; set; }

    }
}
