using Microsoft.Extensions.Configuration;
using OffersAPIClient.Utils.Models;
using OffersAPIClient.Communication;
using System;
using System.Threading.Tasks;

namespace OffersAPIClient.Service
{
    public class FedXAPIClient : IOfferClient
    {
        private IConfiguration _configuration { get; }
        private readonly IRestClient _restClient;

        public FedXAPIClient(IConfiguration configuration, IRestClient restClient)
        {
            _configuration = configuration;
            _restClient = restClient;
        }

        public async Task<BestOfferResponse> GetOfferAsync(BestOfferRequest request)
        {
            var baseUrl = _configuration.GetValue<string>("FedXAPIConfig:BaseURL");
            var apiKey = _configuration.GetValue<string>("FedXAPIConfig:ApiKey");
            var offerResponse = new BestOfferResponse();
            offerResponse.CompanyName = _configuration.GetValue<string>("FedXAPIConfig:CompanyName");

            var postData = new FedXAPIRequest
            {
                Consignee = request.Source,
                Consignor = request.Destination,
                Cartons = request.Carton
            };

            var response = await _restClient.PostRequestAsync<object, FedXAPIResponse>(baseUrl, postData, apiKey);
            if (response == default(FedXAPIResponse))
                offerResponse.BestPrice = decimal.Zero;
            else
                offerResponse.BestPrice = response.Amount;

            return offerResponse;
        }
    }
}
