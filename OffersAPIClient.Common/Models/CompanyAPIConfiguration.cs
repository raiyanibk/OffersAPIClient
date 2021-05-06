using System;
using System.Collections.Generic;
using System.Text;

namespace OffersAPIClient.Common.Models
{
    public class CompanyAPIConfiguration
    {
        public string CompanyName { get; set; }
        public string BaseURL { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
