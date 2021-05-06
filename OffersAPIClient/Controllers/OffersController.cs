﻿using Microsoft.AspNetCore.Mvc;
using OffersAPIClient.Business.Interface;
using OffersAPIClient.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OffersAPIClient.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OffersController : ControllerBase
    {
        protected readonly IOffersService _offersService;
        public OffersController(IOffersService offersService)
        {
            _offersService = offersService;
        }

        [HttpPost]
        [Route("getbestdeal")]
        public async Task<IActionResult> GetBestDeal(BestOfferRequest request)
        {
            var bestDeal = await _offersService.GetBestOffer(request);

            return Ok(bestDeal);
        }
    }
}