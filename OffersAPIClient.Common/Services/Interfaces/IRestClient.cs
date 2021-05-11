using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OffersAPIClient.Common.Service.Interface
{
    public interface IRestClient
    {
        Task<TOut> PostRequest<TIn, TOut>(string uri, TIn content);
        Task<TOut> PostXMLRequest<TIn, TOut>(string uri, TIn content);
    }
}
