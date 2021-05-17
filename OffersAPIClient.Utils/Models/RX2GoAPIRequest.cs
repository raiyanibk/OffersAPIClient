namespace OffersAPIClient.Utils.Models
{
    public class RX2GoAPIRequest
    {
        public string SourceAddress { get; set; }
        public string DestinationAddress { get; set; }
        public int[] CartonDismensions { get; set; }
    }
}
