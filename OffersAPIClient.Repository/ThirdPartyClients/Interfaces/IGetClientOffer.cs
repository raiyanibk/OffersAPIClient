using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OffersAPIClient.Common.Extension;
using OffersAPIClient.Common.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace OffersAPIClient.Repository.ThirdPartyClients
{
    public interface IGetClientOffer
    {
        Task<BestOfferResponse> GetOffer(BestOfferRequest request);
    }

}
