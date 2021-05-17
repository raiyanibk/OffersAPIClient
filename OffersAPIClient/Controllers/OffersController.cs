using Microsoft.AspNetCore.Mvc;
using OffersAPIClient.Utils.Models;
using OffersAPIClient.Service;
using System.Threading.Tasks;

namespace OffersAPIClient.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OffersController : ControllerBase
    {
        protected readonly IOffersService _offersService;
        public OffersController(IOffersService offersService)
        {
            _offersService = offersService;
        }

        [HttpPost]
        [Route("getbestdeal")]
        public async Task<IActionResult> GetBestDealAsync(BestOfferRequest request)
        {
            var bestDeal = await _offersService.GetBestDealAsync(request);

            if (bestDeal == null)
                return NotFound("No correct deal found");

            return Ok(bestDeal);
        }
    }
}
