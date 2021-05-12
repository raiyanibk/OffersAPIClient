using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OffersAPIClient.Utils.Extension;
using OffersAPIClient.Utils.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace OffersAPIClient.Service
{
    public interface IGetClientOffer
    {
        Task<BestOfferResponse> GetOffer(BestOfferRequest request);
    }

}
