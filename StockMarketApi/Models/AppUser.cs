using Microsoft.AspNetCore.Identity;

namespace StockMarketApi.Models
{
    public class AppUser : IdentityUser
    {
        public List<Portfolio> Portfolios { get; set; } = [];

    }
}
