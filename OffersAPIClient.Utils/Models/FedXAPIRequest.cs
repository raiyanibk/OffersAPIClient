namespace OffersAPIClient.Utils.Models
{
    public class FedXAPIRequest
    {
        public string Consignee { get; set; }
        public string Consignor { get; set; }
        public int[] Cartons { get; set; }
    }
}
