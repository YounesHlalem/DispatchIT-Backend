using System;
using System.Collections.Generic;
using System.Text;

namespace IT.Netic.DispatchIt.Web.Backend.DataContracts.BaseDto
{
    /// <summary>
    /// Models a Delivery request Entity.
    /// </summary>
    public class DeliveryrequestBaseDto
    {
        public string _Id { get; set; }
        public int deliveryId { get; set; }
        public string status { get; set; }
        public DateTime creationDatetime { get; set; }
        public string pickupAddressId { get; set; }
        public string companyVat { get; set; }
        public int companyId { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string additionalInstructions { get; set; }
        public string packetDimensions { get; set; }
        public DeliveryAddressBaseDto deliveryAddress { get; set; }
        public int claimedBy { get; set; }
        public DateTime expectedpickup { get; set; }
        public DateTime expecteddelivery { get; set; }
        public DateTime estimatedPickupTime { get; set; }
        public DateTime estimatedDeliveryTime { get; set; }
        public bool isVerifiedBySender { get; set; }
        public ProgressBaseDto progress { get; set; }
    }
}
