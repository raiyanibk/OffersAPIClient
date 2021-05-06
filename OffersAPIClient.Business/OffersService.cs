using OffersAPIClient.Business.Interface;
using OffersAPIClient.Common.Models;
using OffersAPIClient.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OffersAPIClient.Business
{
    public class OffersService : IOffersService
    {
        protected readonly IOffersRepository _offersRepository;
        public OffersService(IOffersRepository offersRepository)
        {
            _offersRepository = offersRepository;
        }

        public async Task<BestOfferResponse> GetBestOffer(BestOfferRequest request)
        {
            return await _offersRepository.GetBestOffer(request);
        }

    }
}
