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

            _offersService.Setup(offerService => offerService.GetBestDealAsync(request)).ReturnsAsync(response);

            // Act 
            int expectedResult = 110;
            var bestDeal = await _offerController.GetBestDealAsync(request);

            // Assert
            var actualResult = ((BestOfferResponse)((ObjectResult)bestDeal).Value).BestPrice;
            Assert.AreEqual(actualResult, expectedResult);
        }

        [TestMethod]
        public async Task GetBestDeal_When_No_Offer_Return()
        {
            var request = new BestOfferRequest
            {
                Source = "S1",
                Destination = "D1",
                Carton = new int[] { 4, 4, 4 }
            };

            BestOfferResponse response = null;

            _offersService.Setup(offerService => offerService.GetBestDealAsync(request)).ReturnsAsync(response);

            int expectedResult = 404;
            var actualBestDeal = await _offerController.GetBestDealAsync(request);
            
            Assert.AreEqual(((ObjectResult)actualBestDeal).StatusCode, expectedResult);
        }
    }
}
