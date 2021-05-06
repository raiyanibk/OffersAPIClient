using OffersAPIClient.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OffersAPIClient.Business.Interface
{
    public interface IOffersService
    {
        Task<BestOfferResponse> GetBestOffer(BestOfferRequest request);
    }
}
