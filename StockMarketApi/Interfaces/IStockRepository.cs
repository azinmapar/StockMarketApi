using StockMarketApi.DTOs.Stock;
using StockMarketApi.Helpers;
using StockMarketApi.Models;

namespace StockMarketApi.Interfaces
{
    public interface IStockRepository
    {

        Task<List<Stock>> GetAllAsync(QueryObject query);
        
        Task<Stock?> GetStockBySymbolAsync(string symbol);

        Task<Stock?> GetByIdAsync(int id);

        Task<Stock> CreateAsync(Stock stock);

        Task<Stock?> UpdateAsync(int id, UpdateStockDto updatedStock);

        Task<Stock?> DeleteAsync(int id);

        Task<bool> StockExistsAsync(int id);

    }
}
