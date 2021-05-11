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
        public IConfiguration Configuration { get; }
        private readonly IEnumerable<IGetClientOffer> _getOffers;

        public OffersRepository(IConfiguration configuration, IEnumerable<IGetClientOffer> getOffers)
        {
            Configuration = configuration;
            _getOffers = getOffers;
        }

        public async Task<BestOfferResponse> GetBestDeal(BestOfferRequest request)
        {
            List<BestOfferResponse> listOffers = new List<BestOfferResponse>();

            foreach (var offer in _getOffers)
            {
                listOffers.Add(await offer.GetOffer(request));
            }

            var bestPrice = listOffers.Where(a => a.BestPrice > 0).Min(a => a.BestPrice);
            var bestOffer = listOffers.FirstOrDefault(a => a.BestPrice == bestPrice);

            return bestOffer;
        }
    }
}
