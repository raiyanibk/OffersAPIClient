using OffersAPIClient.Utils.Models;
using System.Threading.Tasks;

namespace OffersAPIClient.Service
{
    public interface IOfferClient
    {
        Task<BestOfferResponse> GetOfferAsync(BestOfferRequest request);
    }

}
