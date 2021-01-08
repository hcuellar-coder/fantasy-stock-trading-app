namespace FantasyStockTradingApp.Models
{
    public class AuthOptions
    {
        public string SecureKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int ExpiresInHours { get; set; }
    }
}
