using OffersAPIClient.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OffersAPIClient.Repository.Interface
{
    public interface IOffersRepository
    {
        Task<BestOfferResponse> GetBestOffer(BestOfferRequest request);
    }
}
