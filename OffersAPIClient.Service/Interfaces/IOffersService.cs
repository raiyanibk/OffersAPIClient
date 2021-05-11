using OffersAPIClient.Common.Models;
using System.Threading.Tasks;

namespace OffersAPIClient.Service.Interface
{
    public interface IOffersService
    {
        Task<BestOfferResponse> GetBestDeal(BestOfferRequest request);
    }
}
