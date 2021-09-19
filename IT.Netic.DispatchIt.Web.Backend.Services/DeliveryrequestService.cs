using AutoMapper;
using IT.Netic.DispatchIt.Web.Backend.DataContracts.BaseDto;
using IT.Netic.DispatchIt.Web.Backend.Domain.Entities;
using IT.Netic.DispatchIt.Web.Backend.Domain.Repository;
using IT.Netic.DispatchIt.Web.Backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;
using System.IO;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http;

namespace IT.Netic.DispatchIt.Web.Backend.Services
{
    public class DeliveryrequestService : IDeliveryrequestService
    {
        private readonly DeliveryrequestRepository _deliveryrequestRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly IMapper _mapper;
        private EventGridClient eventGridClient;
        private string topicHostname;

        public DeliveryrequestService(DeliveryrequestRepository deliveryrequestRepository, IAddressRepository addressRepository, IMapper mapper)
        {
            _deliveryrequestRepository = deliveryrequestRepository;
            _addressRepository = addressRepository;
            _mapper = mapper;

            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            var configuration = configBuilder.Build();

            var topicEndpoint = configuration.GetConnectionString("TopicEndpoint");
            var topicKey = configuration.GetConnectionString("TopicKey");

            topicHostname = new Uri(topicEndpoint).Host;

            TopicCredentials topicCredentials = new TopicCredentials(topicKey);
            eventGridClient = new EventGridClient(topicCredentials);
        }

        public async Task<IActionResult> SendCreateDeliveryEvent(DeliveryrequestBaseDto delivery)
        {
            if (string.IsNullOrEmpty(delivery.status) || string.IsNullOrEmpty(delivery.companyVat) || string.IsNullOrEmpty(delivery.pickupAddressId) || string.IsNullOrEmpty(delivery.packetDimensions) || delivery.deliveryAddress == null)
            {
                return new BadRequestObjectResult("Deliveryrequest is incomplete!");
            }

            CoordinatesService cs = new CoordinatesService();
            Address ad = new Address();
            ad = _addressRepository.GetById(Convert.ToInt32(delivery.pickupAddressId)).Result;
            string[] res = await cs.locationToLonLatAsync($"{ ad.Country } { ad.City } { ad.Zipcode } { ad.Streetname } { ad.Number }");
            delivery.progress = new ProgressBaseDto();
            delivery.progress.longitude = res[0];
            delivery.progress.latitude = res[1];

            try
            {
                EventGridEvent eventGridEvent = new EventGridEvent()
                {
                    Id = Guid.NewGuid().ToString(),
                    EventType = "Deliveries.Create",
                    Data = delivery,
                    EventTime = DateTime.Now,
                    Subject = "deliveries/create",
                    DataVersion = "1.0"
                };
                List<EventGridEvent> eventsList = new List<EventGridEvent>();
                eventsList.Add(eventGridEvent);
                await eventGridClient.PublishEventsAsync(topicHostname, eventsList);
                return new OkObjectResult($"Successfully sent create delivery event");
            }
            catch (Exception)
            {
                return new BadRequestObjectResult("Failed to send event");
            }
        }

        public IActionResult CreateDeliveryrequest(DeliveryrequestBaseDto delivery)
        {
            var entity = _mapper.Map<Deliveryrequest>(delivery);
            var result = _deliveryrequestRepository.CreateDeliveryrequest(entity);

            return new OkObjectResult("Successfully added Deliveryrequest");
        }

        public IActionResult CreateMessage(MessageDto message)
        {
            /*message.User = ;
            message.Read = false;
            message.CreationDate = DateTime.Now;*/
            var entity = _mapper.Map<Message>(message);
            var result = _deliveryrequestRepository.CreateMessage(entity);

            return new OkObjectResult("Successfully added Message");
        }

        public IActionResult GetDeliveryrequestByVat(string company)
        {
            var deliveryrequests = _deliveryrequestRepository.GetAllDeliveryrequestsForCompany(company);

            if (deliveryrequests.Count() == 0)
            {
                return new BadRequestObjectResult("No data found!");
            }
            return new OkObjectResult(deliveryrequests);
        }

        public IActionResult GetDeliveryrequestByCId(int id)
        {
            var deliveryrequests = _deliveryrequestRepository.GetDeliveryrequestForId(id);

            if (deliveryrequests.Count() == 0)
            {
                return new BadRequestObjectResult("No data found!");
            }
            return new OkObjectResult(deliveryrequests);
        }

        public IActionResult UpdateDeliveryrequest(int id, string action)
        {
            if (action == "Approve")
            {
                return new OkObjectResult(_deliveryrequestRepository.UpdateApprovedDelivery(id));
            }
            else if (action == "Reject")
            {
                return new OkObjectResult(_deliveryrequestRepository.UpdateRejectedDelivery(id));
            }

            return new BadRequestObjectResult("no suitable action found");
        }

        public IActionResult GetDelivery(int id)
        {
            var deliveryrequest = _deliveryrequestRepository.GetDelivery(id);

            if (deliveryrequest == null)
            {
                return new BadRequestObjectResult("No data found!");
            }
            return new OkObjectResult(deliveryrequest);
        }

        public IActionResult GetMessagesForUser(string user)
        {
            var messages = _deliveryrequestRepository.GetAllMessagesByUser(user);

            if (messages.Count() == 0)
            {
                return new BadRequestObjectResult("No data found!");
            }
            return new OkObjectResult(messages);
        }

        public IActionResult PutDeliveryrequest(DeliveryrequestBaseDto delivery)
        {
            var entity = _mapper.Map<Deliveryrequest>(delivery);
            var result = _deliveryrequestRepository.UpdateDelivery(entity);

            return new OkObjectResult("Successfully updated Deliveryrequest");
        }

        public IActionResult DeleteDeliveryrequest(int id)
        {
            var result = _deliveryrequestRepository.DeleteDelivery(id);
            return new OkObjectResult("Successfully updated Deliveryrequest");
        }

        public IActionResult DeleteDeliveryrequestByCId(int id)
        {
            var deliveryrequests = _deliveryrequestRepository.GetDeliveryrequestForId(id);

            if (deliveryrequests.Count() == 0)
            {
                return new BadRequestObjectResult("No data found!");
            }

            foreach(Deliveryrequest del in deliveryrequests)
            {
                DeleteDeliveryrequest(del.deliveryId);
            }
            return new OkObjectResult("Deleted");
        }
    }
}
