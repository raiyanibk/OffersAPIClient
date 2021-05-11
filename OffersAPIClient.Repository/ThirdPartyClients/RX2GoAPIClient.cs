using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OffersAPIClient.Common.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OffersAPIClient.Repository.ThirdPartyClients
{
    public class RX2GoAPIClient : IGetClientOffer
    {
        public IConfiguration Configuration { get; }

        public RX2GoAPIClient(IConfiguration configuration)
        {
            Configuration = configuration;
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

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("ApiKey", Configuration["ApiKey"]);
                var content = new StringContent(JsonConvert.SerializeObject(postData), Encoding.UTF8, "application/json");
                var response = await client.PostAsync(baseUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    var dataObjects = JsonConvert.DeserializeObject<RX2GoAPIResponse>(await response.Content.ReadAsStringAsync());

                    offerResponse.BestPrice = dataObjects.total;
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
