using StockMarketApi.Models;

namespace StockMarketApi.Interfaces
{
    public interface IPortfolioRepository
    {

        Task<List<Stock>> GetUserPortfolioAsync(AppUser user);

    }
}
