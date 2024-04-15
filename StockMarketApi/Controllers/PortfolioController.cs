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
    public class PortfolioController(UserManager<AppUser> userManager, IPortfolioRepository portfolioRepo) : ControllerBase
    {

        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly IPortfolioRepository _portfolioRepo = portfolioRepo;

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserPortfolio()
        {
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);
            var userPortfolio = await _portfolioRepo.GetUserPortfolioAsync(appUser);
            return Ok(userPortfolio);
        }


    }
}
