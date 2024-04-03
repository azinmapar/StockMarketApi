using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult GetAll() 
        {
            var stocks = _context.Stocks.ToList().Select(s => s.ToStockDto());

            return Ok(stocks);
        }

       // [Route("api/[controller]/GetStockById")]
        [HttpGet("{id}")]

        public IActionResult GetById(int id) 
        {
            var stock = _context.Stocks.Find(id);

            if (stock == null) 
            {
                return NotFound();
            } else 
            {
                return Ok(stock.ToStockDto());
            }

        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateStockDto createStockDto)
        {

            var stockModel = createStockDto.FromCreateStockDto();
            _context.Stocks.Add(stockModel);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new {id = stockModel.Id}, stockModel.ToStockDto());

        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult UpdateById([FromRoute] int id, [FromBody] UpdateStockDto updatedStock)
        {
            var currentStock = _context.Stocks.FirstOrDefault(s => s.Id == id);
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
            _context.SaveChanges();
            return Ok(currentStock.ToStockDto());
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeleteById([FromRoute] int id) 
        {
            var stock = _context.Stocks.FirstOrDefault(s => s.Id == id);
            if (stock == null)    
            {
                return NotFound("No such stock");
            }

            _context.Stocks.Remove(stock);
            _context.SaveChanges();
            return NoContent();

        }

    }
}
