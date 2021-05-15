using Microsoft.Extensions.Configuration;
using OffersAPIClient.Utils.Models;
using OffersAPIClient.Communication;
using System;
using System.Threading.Tasks;

namespace OffersAPIClient.Service
{
    public class FedXAPIClient : IGetClientOffer
    {
        private IConfiguration Configuration { get; }
        private readonly IRestClient _restClient;

        public FedXAPIClient(IConfiguration configuration, IRestClient restClient)
        {
            Configuration = configuration;
            _restClient = restClient;
        }

        public async Task<BestOfferResponse> GetOffer(BestOfferRequest request)
        {
            var baseUrl = Configuration.GetValue<string>("FedXAPIConfig:BaseURL");
            var offerResponse = new BestOfferResponse();
            offerResponse.CompanyName = Configuration.GetValue<string>("FedXAPIConfig:CompanyName");

            var postData = new
            {
                Consignee = request.Source,
                Consignor = request.Destination,
                Cartons = request.Carton
            };

            var response = await _restClient.PostRequest<object, FedXAPIResponse>(baseUrl, postData);
            if (response == default(FedXAPIResponse))
                throw new Exception($"Something went wrong in {offerResponse.CompanyName} : GetOffer API");
            else
                offerResponse.BestPrice = response.Amount;

            return offerResponse;
        }
    }
}
