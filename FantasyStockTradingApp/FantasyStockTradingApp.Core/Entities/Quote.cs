namespace FantasyStockTradingApp.Core.Entities
{
    public class Quote
    {
        public string Symbol { get; set; }
        public string CompanyName { get; set; }
        public float LatestPrice { get; set; }
        public float Change { get; set; }
        public float ChangePercent { get; set; }
    }
}
