using Microsoft.Extensions.Configuration;
using OffersAPIClient.Common.Extension;
using OffersAPIClient.Common.Models;
using OffersAPIClient.Common.Service.Interface;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OffersAPIClient.Repository.ThirdPartyClients
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
            var baseUrl = Configuration["PremierAPIConfig:BaseURL"];

            var offerResponse = new BestOfferResponse();
            offerResponse.CompanyName = Configuration["PremierAPIConfig:CompanyName"];

            var postData = new PremierAPIRequest
            {
                source = request.Source,
                destination = request.Destination,
                packages = string.Join(",", request.Carton)
            };

            var response = await _restClient.PostXMLRequest<PremierAPIRequest, PremierAPIResponse>(baseUrl, postData);
            if(response == default(PremierAPIResponse))
                throw new Exception($"Something went wrong in {offerResponse.CompanyName} : GetOffer API");
            else
                offerResponse.BestPrice = response.quote;

            return offerResponse;
        }
    }
}
