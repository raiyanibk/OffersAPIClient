using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OffersAPIClient.Controllers;
using OffersAPIClient.Service;
using OffersAPIClient.Utils.Models;
using System;
using System.Threading.Tasks;

namespace OffersAPIClient.Test
{
    [TestClass]
    public class OffersControllerTest
    {
        readonly IServiceProvider _services =
        Program.CreateHostBuilder(new string[] { }).Build().Services;

        [TestMethod]
        public async Task GetBestDeal_Test_1()
        {

            var myService = _services.GetRequiredService<IOffersService>();
            var controller = new OffersController(myService);

            var data = await controller.GetBestDeal(new BestOfferRequest
            {
                Source = "S1",
                Destination = "D1",
                Carton = new int[] { 4, 4, 4 }
            });

            var value = ((Utils.Models.BestOfferResponse)((Microsoft.AspNetCore.Mvc.ObjectResult)data).Value).BestPrice;

            Assert.AreEqual(value, 110);
        }

        [TestMethod]
        public async Task GetBestDeal_Test_2()
        {

            var myService = _services.GetRequiredService<IOffersService>();
            var controller = new OffersController(myService);

            var data = await controller.GetBestDeal(new BestOfferRequest
            {
                Source = "S1",
                Destination = "D1",
                Carton = new int[] { 5, 5, 5 }
            });

            var value = ((BestOfferResponse)((Microsoft.AspNetCore.Mvc.ObjectResult)data).Value).BestPrice;

            Assert.AreEqual(value, 190);
        }

        [TestMethod]
        public async Task GetBestDeal_Test_3()
        {

            var myService = _services.GetRequiredService<IOffersService>();
            var controller = new OffersController(myService);

            var data = await controller.GetBestDeal(new BestOfferRequest
            {
                Source = "S1",
                Destination = "D1",
                Carton = new int[] { 6, 6, 6 }
            });

            var value = ((BestOfferResponse)((Microsoft.AspNetCore.Mvc.ObjectResult)data).Value).BestPrice;

            Assert.AreEqual(value, 290);
        }

        [TestMethod]
        public async Task GetBestDeal_Test_4()
        {

            var myService = _services.GetRequiredService<IOffersService>();
            var controller = new OffersController(myService);

            var data = await controller.GetBestDeal(new BestOfferRequest
            {
                Source = "S2",
                Destination = "D2",
                Carton = new int[] { 6, 6, 6 }
            });

            var value = ((BestOfferResponse)((Microsoft.AspNetCore.Mvc.ObjectResult)data).Value).BestPrice;

            Assert.AreEqual(value, 490);
        }

        [TestMethod]
        public async Task GetBestDeal_Test_5()
        {

            var myService = _services.GetRequiredService<IOffersService>();
            var controller = new OffersController(myService);

            var data = await controller.GetBestDeal(new BestOfferRequest
            {
                Source = "S2",
                Destination = "D2",
                Carton = new int[] { 5, 5, 5 }
            });

            var value = ((BestOfferResponse)((Microsoft.AspNetCore.Mvc.ObjectResult)data).Value).BestPrice;

            Assert.AreEqual(value, 410);
        }

        [TestMethod]
        public async Task GetBestDeal_Test_6()
        {

            var myService = _services.GetRequiredService<IOffersService>();
            var controller = new OffersController(myService);

            var data = await controller.GetBestDeal(new BestOfferRequest
            {
                Source = "S2",
                Destination = "D2",
                Carton = new int[] { 4, 4, 4 }
            });

            var value = ((BestOfferResponse)((Microsoft.AspNetCore.Mvc.ObjectResult)data).Value).BestPrice;

            Assert.AreEqual(value, 310);
        }
    }
}
