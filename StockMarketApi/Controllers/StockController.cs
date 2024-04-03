using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockMarketApi.Data;
using StockMarketApi.DTOs.Stock;
using StockMarketApi.Mappers;

namespace StockMarketApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class StockController : ControllerBase
    {

        private readonly ApplicationDbContext _context;

        public StockController(ApplicationDbContext context)
        {
            _context = context;
        }

       // [Route("api/[controller]/GetAllStocks")]
        [HttpGet]
        public async Task<IActionResult> GetAll() 
        {
            var stocks = await _context.Stocks.ToListAsync();
            var stockDto = stocks.Select(s => s.ToStockDto());

            return Ok(stocks);
        }

       // [Route("api/[controller]/GetStockById")]
        [HttpGet("{id}")]

        public async Task<IActionResult> GetById(int id) 
        {
            var stock = await _context.Stocks.FindAsync(id);

            if (stock == null) 
            {
                return NotFound();
            } else 
            {
                return Ok(stock.ToStockDto());
            }

        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockDto createStockDto)
        {

            var stockModel = createStockDto.FromCreateStockDto();
            await _context.Stocks.AddAsync(stockModel);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new {id = stockModel.Id}, stockModel.ToStockDto());

        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateById([FromRoute] int id, [FromBody] UpdateStockDto updatedStock)
        {
            var currentStock = await _context.Stocks.FirstOrDefaultAsync(s => s.Id == id);
            if (currentStock == null)
            {
                return NotFound("No such stock");
            }

            currentStock.Symbol = updatedStock.Symbol;
            currentStock.CompanyName = updatedStock.CompanyName;
            currentStock.Purchase = updatedStock.Purchase;
            currentStock.LastDiv = updatedStock.LastDiv;
            currentStock.Industry = updatedStock.Industry;
            currentStock.MarketCap = updatedStock.MarketCap;
            await _context.SaveChangesAsync();
            return Ok(currentStock.ToStockDto());
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteById([FromRoute] int id) 
        {
            var stock = await _context.Stocks.FirstOrDefaultAsync(s => s.Id == id);
            if (stock == null)    
            {
                return NotFound("No such stock");
            }

            _context.Stocks.Remove(stock);
            await _context.SaveChangesAsync();
            return NoContent();

        }

    }
}
