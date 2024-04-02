using StockMarketApi.DTOs.Stock;
using StockMarketApi.Models;

namespace StockMarketApi.Mappers
{
    public static class StockMapper
    {

        public static StockDto ToStockDto(this Stock stockModel)
        {

            return new StockDto
            {
                Id = stockModel.Id,
                Symbol = stockModel.Symbol,
                CompanyName = stockModel.CompanyName,
                Purchase = stockModel.Purchase,
                LastDiv = stockModel.LastDiv,
                Industry = stockModel.Industry,
                MarketCap = stockModel.MarketCap,
            };

        }

        public static Stock FromCreateStockDto(this CreateStockDto createStockDto) 
        {
            return new Stock
            {
                Symbol = createStockDto.Symbol,
                CompanyName = createStockDto.CompanyName,
                Purchase = createStockDto.Purchase,
                LastDiv = createStockDto.LastDiv,
                Industry = createStockDto.Industry,
                MarketCap = createStockDto.MarketCap,
            };
        }

    }
}
