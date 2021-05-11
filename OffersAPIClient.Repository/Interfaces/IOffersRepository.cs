using OffersAPIClient.Common.Models;
using System.Threading.Tasks;

namespace OffersAPIClient.Repository.Interface
{
    public interface IOffersRepository
    {
        Task<BestOfferResponse> GetBestDeal(BestOfferRequest request);
    }
}
