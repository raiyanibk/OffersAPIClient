using Microsoft.Extensions.Configuration;
using OffersAPIClient.Utils.Models;
using OffersAPIClient.Communication;
using System;
using System.Net.Mime;
using System.Threading.Tasks;

namespace OffersAPIClient.Service
{
    public class PremierAPIClient : IGetClientOffer
    {
        private IConfiguration Configuration { get; }
        private readonly IRestClient _restClient;

        public PremierAPIClient(IConfiguration configuration, IRestClient restClient)
        {
            Configuration = configuration;
            _restClient = restClient;
        }

        public async Task<BestOfferResponse> GetOffer(BestOfferRequest request)
        {
            var baseUrl = Configuration.GetValue<string>("PremierAPIConfig:BaseURL");

            var offerResponse = new BestOfferResponse();
            offerResponse.CompanyName = Configuration.GetValue<string>("PremierAPIConfig:CompanyName");

            var postData = new PremierAPIRequest
            {
                Source = request.Source,
                Destination = request.Destination,
                Packages = string.Join(",", request.Carton)
            };

            var response = await _restClient.PostRequest<PremierAPIRequest, PremierAPIResponse>(baseUrl, postData, MediaTypeNames.Application.Xml);
            
            if (response == default(PremierAPIResponse))
                offerResponse.BestPrice = decimal.Zero;
            else
                offerResponse.BestPrice = response.Quote;

            return offerResponse;
        }
    }
}
