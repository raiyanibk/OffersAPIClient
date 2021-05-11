using System;
using System.Collections.Generic;
using System.Text;

namespace OffersAPIClient.Common.Models
{
    public class PremierAPIRequest
    {
        public string source { get; set; }
        public string destination { get; set; }
        public string packages { get; set; }
    }

    public class BestOfferResponse
    {
        public string CompanyName { get; set; }
        public decimal BestPrice { get; set; }
    }

    public class RX2GoAPIResponse
    {
        public decimal total { get; set; }
    }

    public class FedXAPIResponse
    {
        public decimal amount { get; set; }
    }

    public class PremierAPIResponse
    {
        public decimal quote { get; set; }
    }
}
