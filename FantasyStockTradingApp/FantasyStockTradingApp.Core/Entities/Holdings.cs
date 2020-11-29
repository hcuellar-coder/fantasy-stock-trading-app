using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FantasyStockTradingApp.Core.Entities
{
    public class Holdings
    {
        public virtual int Id { get; set; }
        public virtual int AccountId { get; set; }
        public virtual string CompanyName { get; set; }
        public virtual string Symbol { get; set; }
        public virtual int StockCount { get; set; }
        public virtual float LatestCostPerStock { get; set; }
        public virtual float Change { get; set; }
        public virtual float ChangePercentage {get; set;}
        public virtual DateTime LastUpdated { get; set; }

    }
}
