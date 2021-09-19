using System;
using System.Collections.Generic;
using System.Text;

namespace IT.Netic.DispatchIt.Web.Backend.DataContracts.BaseDto
{
    /// <summary>
    /// Models a Address Entity based on the basic address model.
    /// Used specificaly to hold a delivery address
    /// </summary>
    public class DeliveryAddressBaseDto : BasicAddressBaseDto
    {
        public override string ToString()
        {
            return base.ToString();
        }
    }
}
