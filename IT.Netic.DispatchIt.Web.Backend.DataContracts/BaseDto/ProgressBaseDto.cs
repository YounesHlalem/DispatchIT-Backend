using System;
using System.Collections.Generic;
using System.Text;

namespace IT.Netic.DispatchIt.Web.Backend.DataContracts.BaseDto
{
    /// <summary>
    /// Models a Progress Entity.
    /// Used in the deliveryrequest model.
    /// </summary>
    public class ProgressBaseDto
    {
        public DateTime actualPickupTime { get; set; }
        public string longitude { get; set; }
        public string latitude { get; set; }
        public DateTime actualDeliveryTime { get; set; }
    }
}
