using System;
using System.Collections.Generic;
using System.Text;

namespace OffersAPIClient.Common.Models
{
    public class BestOfferRequest
    {
        public string Source { get; set; }
        public string Destination { get; set; }
        public int[] Carton { get; set; }
    }
}
