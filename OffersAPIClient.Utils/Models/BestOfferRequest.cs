namespace OffersAPIClient.Utils.Models
{
    public class BestOfferRequest
    {
        public string Source { get; set; }
        public string Destination { get; set; }
        public int[] Carton { get; set; }
    }
}
