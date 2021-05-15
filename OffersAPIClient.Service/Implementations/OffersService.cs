using Microsoft.Extensions.Configuration;
using OffersAPIClient.Utils.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OffersAPIClient.Service
{
    public class OffersService : IOffersService
    {
        private readonly IEnumerable<IGetClientOffer> _getOffers;

        public OffersService(IEnumerable<IGetClientOffer> getOffers)
        {
            _getOffers = getOffers;
        }

        public async Task<BestOfferResponse> GetBestDeal(BestOfferRequest request)
        {
            List<Task<BestOfferResponse>> listOffers = new List<Task<BestOfferResponse>>();

            foreach (var offer in _getOffers)
            {
                listOffers.Add(offer.GetOffer(request));
            }

            var listOffersData = await Task.WhenAll(listOffers);
            var bestPrice = listOffersData.Where(a => a.BestPrice > 0).Min(a => a.BestPrice);
            var bestOffer = listOffersData.FirstOrDefault(a => a.BestPrice == bestPrice);

            return bestOffer;
        }
    }
}
