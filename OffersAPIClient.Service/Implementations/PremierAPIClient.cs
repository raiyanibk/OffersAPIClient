using Microsoft.Extensions.Configuration;
using OffersAPIClient.Communication;
using OffersAPIClient.Utils.Models;
using System.Net.Mime;
using System.Threading.Tasks;

namespace OffersAPIClient.Service
{
    public class PremierAPIClient : IOfferClient
    {
        private IConfiguration _configuration { get; }
        private readonly IRestClient _restClient;

        public PremierAPIClient(IConfiguration configuration, IRestClient restClient)
        {
            _configuration = configuration;
            _restClient = restClient;
        }

        public async Task<BestOfferResponse> GetOfferAsync(BestOfferRequest request)
        {
            var baseUrl = _configuration.GetValue<string>("PremierAPIConfig:BaseURL");
            var apiKey = _configuration.GetValue<string>("PremierAPIConfig:ApiKey");

            var offerResponse = new BestOfferResponse();
            offerResponse.CompanyName = _configuration.GetValue<string>("PremierAPIConfig:CompanyName");

            var postData = new PremierAPIRequest
            {
                Source = request.Source,
                Destination = request.Destination,
                Packages = string.Join(",", request.Carton)
            };

            var response = await _restClient.PostRequestAsync<PremierAPIRequest, PremierAPIResponse>(baseUrl, postData, apiKey, MediaTypeNames.Application.Xml);

            if (response == default(PremierAPIResponse))
                offerResponse.BestPrice = decimal.Zero;
            else
                offerResponse.BestPrice = response.Quote;

            return offerResponse;
        }
    }
}
