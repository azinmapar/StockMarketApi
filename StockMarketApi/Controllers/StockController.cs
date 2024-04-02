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
        [HttpGet("id")]

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

    }
}
