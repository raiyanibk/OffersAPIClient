using OffersAPIClient.Utils.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OffersAPIClient.Service
{
    public class OffersService : IOffersService
    {
        private readonly IEnumerable<IOfferClient> _offerClients;

        public OffersService(IEnumerable<IOfferClient> getOffers)
        {
            _offerClients = getOffers;
        }

        public async Task<BestOfferResponse> GetBestDealAsync(BestOfferRequest request)
        {
            List<Task<BestOfferResponse>> listOffers = new List<Task<BestOfferResponse>>();

            foreach (var offerClient in _offerClients)
            {
                listOffers.Add(offerClient.GetOfferAsync(request));
            }

            var listOffersData = await Task.WhenAll(listOffers);

            // Negative value handling
            var bestPrice = listOffersData.Where(a => a != null && a.BestPrice > 0);
            decimal bestPriceValue = decimal.Zero;
            if(bestPrice.Count() > 0)
            {
                bestPriceValue = bestPrice.Min(a => a.BestPrice);
            }

            var bestOffer = listOffersData.FirstOrDefault(a => a.BestPrice == bestPriceValue);

            return bestOffer;
        }
    }
}
