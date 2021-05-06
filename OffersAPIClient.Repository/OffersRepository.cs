using OffersAPIClient.Common.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OffersAPIClient.Repository.Interface;
using OffersAPIClient.Common.Extension;
using System.Linq;
using OffersAPIClient.Common;

namespace OffersAPIClient.Repository
{
    public class OffersRepository : IOffersRepository
    {
        public IConfiguration Configuration { get; }

        public OffersRepository(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public async Task<BestOfferResponse> GetBestOffer(BestOfferRequest request)
        {
            List<BestOfferResponse> listOffers = new List<BestOfferResponse>();

            listOffers.Add(new BestOfferResponse
            {
                BestPrice = await CallRX2GoAPI(request),
                CompanyName = CompanyName.RX2Go
            });
            listOffers.Add(new BestOfferResponse
            {
                BestPrice = await CallFedXAPI(request),
                CompanyName = CompanyName.FedX
            });
            listOffers.Add(new BestOfferResponse
            {
                BestPrice = await CallPremierAPI(request),
                CompanyName = CompanyName.Premier
            });

            var minValue = listOffers.Where(a => a.BestPrice > 0).Min(a => a.BestPrice);
            var bestOffer = listOffers.FirstOrDefault(a => a.BestPrice == minValue);

            return bestOffer;
        }

        private async Task<decimal> CallRX2GoAPI(BestOfferRequest request)
        {
            var baseUrl = Configuration["Company1APIConfig:BaseURL"];
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

                    return dataObjects.total;
                }
                else
                {
                    return 0;
                }
            }

        }

        private async Task<decimal> CallFedXAPI(BestOfferRequest request)
        {
            var baseUrl = Configuration["Company2APIConfig:BaseURL"];
            var postData = new
            {
                consignee = request.Source,
                consignor = request.Destination,
                cartons = request.Carton
            };

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("ApiKey", Configuration["ApiKey"]);
                var content = new StringContent(JsonConvert.SerializeObject(postData), Encoding.UTF8, "application/json");
                var response = await client.PostAsync(baseUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    var dataObjects = JsonConvert.DeserializeObject<FedXAPIResponse>(await response.Content.ReadAsStringAsync());

                    return dataObjects.amount;
                }
                else
                {
                    return 0;
                }
            }
        }

        private async Task<decimal> CallPremierAPI(BestOfferRequest request)
        {
            var baseUrl = Configuration["Company3APIConfig:BaseURL"];
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

                    return dataObjects.quote;
                }
                else
                {
                    return 0;
                }
            }
        }
    }
}
