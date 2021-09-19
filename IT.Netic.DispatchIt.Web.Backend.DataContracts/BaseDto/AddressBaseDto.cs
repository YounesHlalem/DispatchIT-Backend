using System;
using System.Collections.Generic;
using System.Text;

namespace IT.Netic.DispatchIt.Web.Backend.DataContracts.BaseDto
{
    /// <summary>
    /// Models a Address Entity based on the basic address model.
    /// Used specificaly to hold a company address.
    /// </summary>
    public class AddressBaseDto : BasicAddressBaseDto
    {
        public int AddressId { get; set; }
        public int CompanyId { get; set; }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
