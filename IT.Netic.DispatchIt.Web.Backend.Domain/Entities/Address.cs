using System;
using System.Collections.Generic;
using System.Text;

namespace IT.Netic.DispatchIt.Web.Backend.Domain.Entities
{
    /// <summary>
    /// Models a Address Entity based on the basic address model.
    /// Used specificaly to hold a company address.
    /// </summary>
    public class Address : BasicAddress
    {
        public int AddressId { get; set; }
        public int CompanyId { get; set; }
    }
}
