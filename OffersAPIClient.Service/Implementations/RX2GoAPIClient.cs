using Microsoft.Extensions.Configuration;
using OffersAPIClient.Communication;
using OffersAPIClient.Utils.Models;
using System.Threading.Tasks;

namespace OffersAPIClient.Service
{
    public class RX2GoAPIClient : IOfferClient
    {
        private IConfiguration _configuration { get; }
        private readonly IRestClient _restClient;

        public RX2GoAPIClient(IConfiguration configuration, IRestClient restClient)
        {
            _configuration = configuration;
            _restClient = restClient;
        }

        public async Task<BestOfferResponse> GetOfferAsync(BestOfferRequest request)
        {
            var baseUrl = _configuration.GetValue<string>("RX2GoAPIConfig:BaseURL");
            var apiKey = _configuration.GetValue<string>("RX2GoAPIConfig:ApiKey");
            var offerResponse = new BestOfferResponse();
            offerResponse.CompanyName = _configuration.GetValue<string>("RX2GoAPIConfig:CompanyName");

            var postData = new RX2GoAPIRequest
            {
                SourceAddress = request.Source,
                DestinationAddress = request.Destination,
                CartonDismensions = request.Carton
            };

            var response = await _restClient.PostRequestAsync<object, RX2GoAPIResponse>(baseUrl, postData, apiKey);

            if (response == default(RX2GoAPIResponse))
                offerResponse.BestPrice = decimal.Zero;
            else
                offerResponse.BestPrice = response.Total;

            return offerResponse;
        }
    }
}
