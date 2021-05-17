using OffersAPIClient.Utils.Models;
using System.Threading.Tasks;

namespace OffersAPIClient.Service
{
    public interface IOffersService
    {
        Task<BestOfferResponse> GetBestDealAsync(BestOfferRequest request);
    }
}
