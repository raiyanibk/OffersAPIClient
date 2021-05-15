using System;
using System.Collections.Generic;
using System.Text;

namespace OffersAPIClient.Utils.Models
{
    public class PremierAPIRequest
    {
        public string Source { get; set; }
        public string Destination { get; set; }
        public string Packages { get; set; }
    }

    public class BestOfferResponse
    {
        public string CompanyName { get; set; }
        public decimal BestPrice { get; set; }
    }

    public class RX2GoAPIResponse
    {
        public decimal Total { get; set; }
    }

    public class FedXAPIResponse
    {
        public decimal Amount { get; set; }
    }

    public class PremierAPIResponse
    {
        public decimal Quote { get; set; }
    }
}
