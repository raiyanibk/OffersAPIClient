using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OffersAPIClient.Controllers;
using OffersAPIClient.Service;
using OffersAPIClient.Utils.Models;
using System;
using System.Threading.Tasks;
using Moq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace OffersAPIClient.Test
{
    [TestClass]
    public class OffersControllerMoqTest
    {
        private readonly OffersController _offerController;
        private readonly Mock<IOffersService> _offersService = new Mock<IOffersService>();
        public OffersControllerMoqTest()
        {
            _offerController = new OffersController(_offersService.Object);
        }

        [TestMethod]
        public async Task GetBestDeal_ShouldReturnBestDealDetail()
        {
            // Arrange
            var request = new BestOfferRequest
            {
                Source = "S1",
                Destination = "D1",
                Carton = new int[] { 4, 4, 4 }
            };

            var response = new BestOfferResponse { CompanyName = "FedX", BestPrice = 110 };

            _offersService.Setup(a => a.GetBestDeal(request)).ReturnsAsync(response);

            // Act 
            var getBestDeal = await _offerController.GetBestDeal(request);

            // Assert
            var value = ((BestOfferResponse)((ObjectResult)getBestDeal).Value).BestPrice;
            Assert.AreEqual(value, 110);
        }
    }
}
