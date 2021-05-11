using Microsoft.Extensions.Configuration;
using OffersAPIClient.Common.Models;
using OffersAPIClient.Repository.Interface;
using OffersAPIClient.Repository.ThirdPartyClients;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OffersAPIClient.Repository
{
    public class OffersRepository : IOffersRepository
    {
        private IConfiguration Configuration { get; }
        private readonly IEnumerable<IGetClientOffer> _getOffers;

        public OffersRepository(IConfiguration configuration, IEnumerable<IGetClientOffer> getOffers)
        {
            Configuration = configuration;
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
