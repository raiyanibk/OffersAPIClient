using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OffersAPIClient.Utils;
using OffersAPIClient.Utils.Extension;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace OffersAPIClient.Communication
{
    public class RestClient : IRestClient
    {
        public static readonly HttpClient client = new HttpClient();
        private IConfiguration _config { get; }

        public RestClient(IConfiguration configuration)
        {
            _config = configuration;
        }

        public async Task<TOut> PostRequestAsync<TIn, TOut>(string uri, TIn content, string apiKey, string mediaType)
        {
            StringContent serialized = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, mediaType);

            return await SendRequestAsync<TOut>(uri, serialized, apiKey);
        }

        private async Task<TOut> SendRequestAsync<TOut>(string uri, StringContent postData, string apiKey)
        {
            if (!client.DefaultRequestHeaders.Contains(ConfigKey.ApiKey))
                client.DefaultRequestHeaders.Add(ConfigKey.ApiKey, apiKey);

            var MaxRetries = _config.GetValue<int>(ConfigKey.ReTryCount);

            for (int i = 0; i < MaxRetries; i++)
            {
                using (HttpResponseMessage response = await client.PostAsync(uri, postData))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        TOut dataObjects;
                        if (response.Content.Headers.ContentType.MediaType == MediaTypeNames.Application.Xml)
                        {
                            dataObjects = Extension.Deserialize<TOut>(await response.Content.ReadAsStringAsync());
                        }
                        else
                        {
                            dataObjects = JsonConvert.DeserializeObject<TOut>(await response.Content.ReadAsStringAsync());
                        }

                        return dataObjects;
                    }
                }
            }

            return default(TOut);
        }
    }
}
