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
using Microsoft.Extensions.Configuration;
using OffersAPIClient.Communication;

namespace OffersAPIClient.Test
{
    [TestClass]
    public class OffersServiceMoqTest
    {
        private OffersService _offersService;
        private readonly Mock<IEnumerable<IOfferClient>> mock = new Mock<IEnumerable<IOfferClient>>();

        private readonly FedXAPIClient _fedxapiclient;
        private readonly PremierAPIClient _premierapiclient;
        private readonly RX2GoAPIClient _rx2goapiclient;
        private readonly IList<IOfferClient> _clientOffers;

        private readonly Mock<IConfiguration> _config = new Mock<IConfiguration>();
        private readonly Mock<IRestClient> _restClient = new Mock<IRestClient>();
        private readonly Mock<IOfferClient> _fedXAPIClientMock;
        private readonly Mock<IOfferClient> _premierAPIClientMock;
        private readonly Mock<IOfferClient> _rX2GoAPIClientMock;
        public OffersServiceMoqTest()
        {
            _fedXAPIClientMock = new Mock<IOfferClient>();
            _premierAPIClientMock = new Mock<IOfferClient>();
            _rX2GoAPIClientMock = new Mock<IOfferClient>();

            _clientOffers = new List<IOfferClient> { _fedXAPIClientMock.Object, _premierAPIClientMock.Object, _rX2GoAPIClientMock.Object };
            _offersService = new OffersService(_clientOffers);
        }

        [TestMethod]
        public async Task GetBestDeal_When_AllCompanies_Are_Returning_Response()
        {

            // Arrange
            var request = new BestOfferRequest
            {
                Source = "S1",
                Destination = "D1",
                Carton = new int[] { 4, 4, 4 }
            };

            var fedxResponse = new BestOfferResponse
            {
                BestPrice = 110,
                CompanyName = "FedX"
            };

            var premierResponse = new BestOfferResponse
            {
                BestPrice = 100,
                CompanyName = "Premier"
            };

            var rx2Response = new BestOfferResponse
            {
                BestPrice = 120,
                CompanyName = "Rx2Go"
            };

            SetupGetOfferMethod(request, fedxResponse, premierResponse, rx2Response);


            // Act 
            int expectedValue = 100;
            var bestDeal = await _offersService.GetBestDealAsync(request);

            // Assert
            var actualValue = bestDeal.BestPrice;
            Assert.AreEqual(actualValue, expectedValue);
        }

        [TestMethod]
        public async Task GetBestDeal_When_API_OneCompany_ReturningZero()
        {

            // Arrange
            var request = new BestOfferRequest
            {
                Source = "S1",
                Destination = "D1",
                Carton = new int[] { 4, 4, 4 }
            };

            var fedxResponse = new BestOfferResponse
            {
                BestPrice = 0,
                CompanyName = "FedX"
            };

            var premierResponse = new BestOfferResponse
            {
                BestPrice = 90,
                CompanyName = "Premier"
            };

            var rx2Response = new BestOfferResponse
            {
                BestPrice = 120,
                CompanyName = "Rx2Go"
            };

            SetupGetOfferMethod(request, fedxResponse, premierResponse, rx2Response);

            // Act 
            int expectedValue = 90;
            var bestDeal = await _offersService.GetBestDealAsync(request);

            // Assert
            var actualValue = bestDeal.BestPrice;
            Assert.AreEqual(actualValue, expectedValue);
        }

        [TestMethod]
        public async Task GetBestDeal_When_API_OneCompany_ReturningNull()
        {

            // Arrange
            var request = new BestOfferRequest
            {
                Source = "S1",
                Destination = "D1",
                Carton = new int[] { 4, 4, 4 }
            };

            BestOfferResponse fedxResponse = null;

            var premierResponse = new BestOfferResponse
            {
                BestPrice = 90,
                CompanyName = "Premier"
            };

            var rx2Response = new BestOfferResponse
            {
                BestPrice = 120,
                CompanyName = "Rx2Go"
            };

            SetupGetOfferMethod(request, fedxResponse, premierResponse, rx2Response);

            // Act 
            int expectedValue = 90;
            var bestDeal = await _offersService.GetBestDealAsync(request);

            // Assert
            var actualValue = bestDeal.BestPrice;
            Assert.AreEqual(actualValue, expectedValue);
        }

        [TestMethod]
        public async Task GetBestDeal_When_API_ReturningNegativeValue()
        {

            // Arrange
            var request = new BestOfferRequest
            {
                Source = "S1",
                Destination = "D1",
                Carton = new int[] { 4, 4, 4 }
            };

            BestOfferResponse fedxResponse = new BestOfferResponse
            {
                BestPrice = -80,
                CompanyName = "Premier"
            }; ;

            var premierResponse = new BestOfferResponse
            {
                BestPrice = -100,
                CompanyName = "Premier"
            };

            var rx2Response = new BestOfferResponse
            {
                BestPrice = -120,
                CompanyName = "Rx2Go"
            };

            SetupGetOfferMethod(request, fedxResponse, premierResponse, rx2Response);

            // Act 
            int expectedValue = 90;
            var bestDeal = await _offersService.GetBestDealAsync(request);

            // Assert
            var actualValue = bestDeal.BestPrice;
            Assert.AreEqual(actualValue, expectedValue);
        }

        private void SetupGetOfferMethod(BestOfferRequest request, BestOfferResponse fedxResponse, BestOfferResponse premierResponse, BestOfferResponse rx2Response)
        {
            _fedXAPIClientMock.Setup(a => a.GetOfferAsync(request)).ReturnsAsync(fedxResponse);
            _premierAPIClientMock.Setup(a => a.GetOfferAsync(request)).ReturnsAsync(premierResponse);
            _rX2GoAPIClientMock.Setup(a => a.GetOfferAsync(request)).ReturnsAsync(rx2Response);
        }
    }
}
