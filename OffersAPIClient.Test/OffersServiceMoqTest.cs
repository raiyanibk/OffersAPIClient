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
        private readonly Mock<FedXAPIClient> _fedXAPIClientMock = new Mock<FedXAPIClient>();
        private readonly Mock<PremierAPIClient> _premierAPIClientMock = new Mock<PremierAPIClient>();
        private readonly Mock<RX2GoAPIClient> _rX2GoAPIClientMock = new Mock<RX2GoAPIClient>();
        public OffersServiceMoqTest()
        {
            _fedxapiclient = new FedXAPIClient(_config.Object, _restClient.Object);
            _premierapiclient = new PremierAPIClient(_config.Object, _restClient.Object);
            _rx2goapiclient = new RX2GoAPIClient(_config.Object, _restClient.Object);
            _clientOffers = new List<IOfferClient> { _fedxapiclient, _premierapiclient, _rx2goapiclient };
            _offersService = new OffersService(_clientOffers);

            _fedXAPIClientMock = new Mock<FedXAPIClient>(_config.Object, _restClient.Object);
            _premierAPIClientMock = new Mock<PremierAPIClient>(_config.Object, _restClient.Object);
            _rX2GoAPIClientMock = new Mock<RX2GoAPIClient>(_config.Object, _restClient.Object);
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

            _config.Setup(c => c.GetSection(It.IsAny<String>())).Returns(new Mock<IConfigurationSection>().Object);

            _fedXAPIClientMock.Setup(a => a.GetOfferAsync(request)).ReturnsAsync(fedxResponse);
            _premierAPIClientMock.Setup(a => a.GetOfferAsync(request)).ReturnsAsync(premierResponse);
            _rX2GoAPIClientMock.Setup(a => a.GetOfferAsync(request)).ReturnsAsync(rx2Response);


            // Act 
            var getBestDeal = await _offersService.GetBestDealAsync(request);

            // Assert
            var value = getBestDeal.BestPrice;
            Assert.AreEqual(value, 110);
        }

        
    }
}
