using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OffersAPIClient.Common.Models;
using OffersAPIClient.Common.Service.Interface;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OffersAPIClient.Repository.ThirdPartyClients
{
    public class RX2GoAPIClient : IGetClientOffer
    {
        private IConfiguration Configuration { get; }
        private readonly IRestClient _restClient;

        public RX2GoAPIClient(IConfiguration configuration, IRestClient restClient)
        {
            Configuration = configuration;
            _restClient = restClient;
        }

        public async Task<BestOfferResponse> GetOffer(BestOfferRequest request)
        {
            var baseUrl = Configuration["RX2GoAPIConfig:BaseURL"];
            var offerResponse = new BestOfferResponse();
            offerResponse.CompanyName = Configuration["RX2GoAPIConfig:CompanyName"];

            var postData = new
            {
                SourceAddress = request.Source,
                DestinationAddress = request.Destination,
                CartonDismensions = request.Carton
            };

            var response = await _restClient.PostRequest<object, RX2GoAPIResponse>(baseUrl, postData);
            if (response == default(RX2GoAPIResponse))
                throw new Exception($"Something went wrong in {offerResponse.CompanyName} : GetOffer API");
            else
                offerResponse.BestPrice = response.total;

            return offerResponse;
        }
    }
}
