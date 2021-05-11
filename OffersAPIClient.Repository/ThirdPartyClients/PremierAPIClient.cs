using Microsoft.Extensions.Configuration;
using OffersAPIClient.Common.Extension;
using OffersAPIClient.Common.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OffersAPIClient.Repository.ThirdPartyClients
{
    public class PremierAPIClient : IGetClientOffer
    {
        public IConfiguration Configuration { get; }

        public PremierAPIClient(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public async Task<BestOfferResponse> GetOffer(BestOfferRequest request)
        {
            var baseUrl = Configuration["PremierAPIConfig:BaseURL"];

            var offerResponse = new BestOfferResponse();
            offerResponse.CompanyName = Configuration["PremierAPIConfig:CompanyName"];

            var postData = new API3Request
            {
                source = request.Source,
                destination = request.Destination,
                packages = string.Join(",", request.Carton)
            };

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("ApiKey", Configuration["ApiKey"]);
                var content = new StringContent(Extension.Serialize(postData).ToString(), Encoding.UTF8, "application/xml");
                var response = await client.PostAsync(baseUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    var dataObjects = Extension.Deserialize<PremierAPIResponse>(await response.Content.ReadAsStringAsync());

                    offerResponse.BestPrice = dataObjects.quote;
                }
                else
                {
                    throw new Exception($"Something went wrong in {offerResponse.CompanyName} : GetOffer API");
                }
            }

            return offerResponse;
        }
    }
}
