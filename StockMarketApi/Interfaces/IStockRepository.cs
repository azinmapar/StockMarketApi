using StockMarketApi.DTOs.Stock;
using StockMarketApi.Models;

namespace StockMarketApi.Interfaces
{
    public interface IStockRepository
    {

        Task<List<Stock>> GetAllAsync();

        Task<Stock?> GetByIdAsync(int id);

        Task<Stock> CreateAsync(Stock stock);

        Task<Stock?> UpdateAsync(int id, UpdateStockDto updatedStock);

        Task<Stock?> DeleteAsync(int id);

        Task<bool> StockExistsAsync(int id);

    }
}
