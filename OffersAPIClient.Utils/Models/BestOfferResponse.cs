using System;
using System.Collections.Generic;
using System.Text;

namespace OffersAPIClient.Utils.Models
{
    public class APIResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
    public class PremierAPIRequest
    {
        public string Source { get; set; }
        public string Destination { get; set; }
        public string Packages { get; set; }
    }

    public class FedXAPIRequest
    {
        public string Consignee { get; set; }
        public string Consignor { get; set; }
        public int[] Cartons { get; set; }
    }
    public class RX2GoAPIRequest
    {
        public string SourceAddress { get; set; }
        public string DestinationAddress { get; set; }
        public int[] CartonDismensions { get; set; }
    }

    public class BestOfferResponse
    {
        public string CompanyName { get; set; }
        public decimal BestPrice { get; set; }
    }

    public class RX2GoAPIResponse : APIResponse
    {
        public decimal Total { get; set; }
    }

    public class FedXAPIResponse : APIResponse
    {
        public decimal Amount { get; set; }
    }

    public class PremierAPIResponse : APIResponse
    {
        public decimal Quote { get; set; }
    }

    
}
