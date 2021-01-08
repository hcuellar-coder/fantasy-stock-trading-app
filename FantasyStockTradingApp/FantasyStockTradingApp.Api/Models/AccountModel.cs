namespace FantasyStockTradingApp.Models
{
    public class AccountModel
    {
        public virtual int Id { get; set; }
        public virtual int UserId { get; set; }
        public virtual float Balance { get; set; }
        public virtual float PortfolioBalance { get; set; }
    }
}
