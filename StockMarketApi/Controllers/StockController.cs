using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockMarketApi.Data;
using StockMarketApi.DTOs.Stock;
using StockMarketApi.Helpers;
using StockMarketApi.Interfaces;
using StockMarketApi.Mappers;

namespace StockMarketApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class StockController(ApplicationDbContext context, IStockRepository stockRepo) : ControllerBase
    {

        private readonly ApplicationDbContext _context = context;
        private readonly IStockRepository _stockRepo = stockRepo;

        // [Route("api/[controller]/GetAllStocks")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll([FromQuery] QueryObject query) 
        {

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var stocks = await _stockRepo.GetAllAsync(query);
            var stockDto = stocks.Select(s => s.ToStockDto());

            return Ok(stockDto);
        }

       // [Route("api/[controller]/GetStockById")]
        [HttpGet("{id}")]

        public async Task<IActionResult> GetById(int id) 
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var stock = await _stockRepo.GetByIdAsync(id);

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
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }


            var stockModel = createStockDto.FromCreateStockDto();
            await _stockRepo.CreateAsync(stockModel);
            return CreatedAtAction(nameof(GetById), new {id = stockModel.Id}, stockModel.ToStockDto());

        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateById([FromRoute] int id, [FromBody] UpdateStockDto updatedStock)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var currentStock = await _stockRepo.UpdateAsync(id, updatedStock);
            if (currentStock == null)
            {
                return NotFound("No such stock");
            }

            return Ok(currentStock.ToStockDto());
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteById([FromRoute] int id) 
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var stock = await _stockRepo.DeleteAsync(id);
            if (stock == null)    
            {
                return NotFound("No such stock");
            }

            
            return NoContent();

        }

    }
}
