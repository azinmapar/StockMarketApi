using StockMarketApi.Models;

namespace StockMarketApi.Interfaces
{
    public interface IPortfolioRepository
    {

        Task<List<Stock>> GetUserPortfolioAsync(AppUser user);

        Task<Portfolio> CreateAsync(Portfolio portfolio);

        Task<Portfolio?> DeleteAsync(Stock stock, AppUser user);
    }
}
