using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OffersAPIClient.Common.Service.Interface;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OffersAPIClient.Common.Service
{
    public class RestClient : IRestClient
    {
        public static readonly HttpClient client = new HttpClient();
        private IConfiguration _config { get; }

        public RestClient(IConfiguration configuration)
        {
            _config = configuration;
        }

        public async Task<TOut> PostRequest<TIn, TOut>(string uri, TIn content)
        {
            var serialized = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");

            return await SendRequest<TOut>(uri, serialized);
        }

        public async Task<TOut> PostXMLRequest<TIn, TOut>(string uri, TIn content)
        {
            var serialized = new StringContent(Extension.Extension.Serialize(content).ToString(), Encoding.UTF8, "application/xml");

            return await SendRequest<TOut>(uri, serialized);
        }

        private async Task<TOut> SendRequest<TOut>(string uri, StringContent postData)
        {
            if (!client.DefaultRequestHeaders.Contains("ApiKey"))
                client.DefaultRequestHeaders.Add("ApiKey", _config["ApiKey"]);

            var MaxRetries = Convert.ToInt32(_config["ReTryCount"]);

            for (int i = 0; i < MaxRetries; i++)
            {
                using (HttpResponseMessage response = await client.PostAsync(uri, postData))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var dataObjects = Extension.Extension.Deserialize<TOut>(await response.Content.ReadAsStringAsync());

                        return dataObjects;
                    }
                    else if (response.StatusCode == HttpStatusCode.BadRequest)
                    {
                        // Don't reattempt a bad request
                        break;
                    }
                }
            }

            return default(TOut);
        }
    }
}
