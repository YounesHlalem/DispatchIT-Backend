using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace IT.Netic.DispatchIt.Web.Backend.Domain.Entities
{
    /// <summary>
    /// Models a Delivery request Entity.
    /// </summary>
    public class Deliveryrequest
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _Id { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int deliveryId { get; set; }
        //[BsonElement("Status")]
        public string status { get; set; }
        public DateTime creationDatetime { get; set; }
        public string pickupAddressId { get; set; }
        public string companyVat { get; set; }
        public int companyId { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string additionalInstructions { get; set; }
        public string packetDimensions { get; set; }
        public DeliveryAddress deliveryAddress { get; set; }
        public int claimedBy { get; set; }
        public DateTime expectedpickup { get; set; }
        public DateTime expecteddelivery { get; set; }
        public DateTime estimatedPickupTime { get; set; }
        public DateTime estimatedDeliveryTime { get; set; }
        public bool isVerifiedBySender { get; set; }
        public Progress progress { get; set; }
    }
}