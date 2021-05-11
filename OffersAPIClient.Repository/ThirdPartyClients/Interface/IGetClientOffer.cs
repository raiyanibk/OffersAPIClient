using OffersAPIClient.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OffersAPIClient.Repository.ThirdPartyClients
{
    public interface IGetClientOffer
    {
        Task<BestOfferResponse> GetOffer(BestOfferRequest request);
    }
}
