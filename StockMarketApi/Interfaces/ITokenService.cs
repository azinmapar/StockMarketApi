using StockMarketApi.Models;

namespace StockMarketApi.Interfaces
{
    public interface ITokenService
    {

        string CreateToken(AppUser user);

    }
}
