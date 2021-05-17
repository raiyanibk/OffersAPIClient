using Microsoft.Extensions.Configuration;
using OffersAPIClient.Utils.Models;
using OffersAPIClient.Communication;
using System;
using System.Net.Mime;
using System.Threading.Tasks;

namespace OffersAPIClient.Service
{
    public class PremierAPIClient : IOfferClient
    {
        private IConfiguration Configuration { get; }
        private readonly IRestClient _restClient;

        public PremierAPIClient(IConfiguration configuration, IRestClient restClient)
        {
            Configuration = configuration;
            _restClient = restClient;
        }

        public async Task<BestOfferResponse> GetOfferAsync(BestOfferRequest request)
        {
            var baseUrl = Configuration.GetValue<string>("PremierAPIConfig:BaseURL");
            var apiKey = Configuration.GetValue<string>("PremierAPIConfig:ApiKey");

            var offerResponse = new BestOfferResponse();
            offerResponse.CompanyName = Configuration.GetValue<string>("PremierAPIConfig:CompanyName");

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
