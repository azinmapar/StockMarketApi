using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StockMarketApi.Extensions;
using StockMarketApi.Interfaces;
using StockMarketApi.Models;

namespace StockMarketApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PortfolioController(UserManager<AppUser> userManager, IStockRepository stockRepo, IPortfolioRepository portfolioRepo) : ControllerBase
    {

        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly IPortfolioRepository _portfolioRepo = portfolioRepo;
        private readonly IStockRepository _stockRepo = stockRepo;

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserPortfolio()
        {
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);
            var userPortfolio = await _portfolioRepo.GetUserPortfolioAsync(appUser);
            return Ok(userPortfolio);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreatePortfolio(string symbol)
        {
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);
            var stock = await _stockRepo.GetStockBySymbolAsync(symbol);

            if (stock == null) return BadRequest("stock not found");

            var userPortfolio = await _portfolioRepo.GetUserPortfolioAsync(appUser);

            if (userPortfolio.Any(e => e.Symbol.ToLower() == symbol.ToLower()))
            {
                return StatusCode(500, "stock already exists in portfolio");
            }

            var portfolio = new Portfolio
            {
                AppUserId = appUser.Id,
                StockId = stock.Id,
            };



            if (portfolio == null)
            {

                return StatusCode(500, "portfolio not created");
            }
            else {
                await _portfolioRepo.CreateAsync(portfolio); 
            }


            return Created();

        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> Delete(string symbol)
        {
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);
            var portfolios = await _portfolioRepo.GetUserPortfolioAsync(appUser);

            var portfolio = portfolios.FirstOrDefault(e => e.Symbol.ToLower() == symbol.ToLower());

            if ( portfolio == null )
            {
                return NotFound( "no portfolio with this symbol");
            }

            var result = await _portfolioRepo.DeleteAsync(portfolio, appUser);

            if (result == null)
            {
                return NotFound("no portfolio with this symbol");
            }

            return NoContent();
        }

    }
}
