using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace OffersAPIClient.Communication
{
    public interface IRestClient
    {
        Task<TOut> PostRequestAsync<TIn, TOut>(string uri, TIn content, string apiKey, string application = MediaTypeNames.Application.Json);
    }
}
