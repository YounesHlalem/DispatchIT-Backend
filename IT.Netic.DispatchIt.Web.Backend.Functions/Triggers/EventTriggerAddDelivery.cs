using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Extensions.Logging;
using IT.Netic.DispatchIt.Web.Backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.Azure.EventGrid;
using IT.Netic.DispatchIt.Web.Backend.DataContracts.BaseDto;
using System.Security.Claims;

namespace IT.Netic.DispatchIt.Web.Backend.Functions.Triggers
{
    public class EventTriggerAddDelivery
    {
        private readonly IDeliveryrequestService _drService;
        private readonly IValidationService _validationService;

        public EventTriggerAddDelivery(IDeliveryrequestService drService, IValidationService validationService)
        {
            _drService = drService;
            _validationService = validationService;
        }
        [FunctionName("EventTriggerAddDelivery")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation($"C# trigger function begun");
            string response = string.Empty;

                string requestContent = await new StreamReader(req.Body).ReadToEndAsync();
                log.LogInformation($"Received events: {requestContent}");

                EventGridSubscriber eventGridSubscriber = new EventGridSubscriber();
                eventGridSubscriber.AddOrUpdateCustomEventMapping("Deliveries.Create", typeof(DeliveryrequestBaseDto));
                EventGridEvent[] eventGridEvents = eventGridSubscriber.DeserializeEventGridEvents(requestContent);

                foreach (EventGridEvent eventGridEvent in eventGridEvents)
                {
                    if (eventGridEvent.Data is SubscriptionValidationEventData)
                    {
                        var eventData = (SubscriptionValidationEventData)eventGridEvent.Data;
                        log.LogInformation($"Got SubscriptionValidation event data, validation code: {eventData.ValidationCode}, topic: {eventGridEvent.Topic}");

                        var responseData = new SubscriptionValidationResponse()
                        {
                            ValidationResponse = eventData.ValidationCode
                        };

                        return new BadRequestObjectResult(responseData);
                    }
                    else if (eventGridEvent.Data is DeliveryrequestBaseDto)
                    {
                        var eventData = (DeliveryrequestBaseDto)eventGridEvent.Data;
                        var responseData = _drService.CreateDeliveryrequest(eventData);
                        log.LogInformation($"Completed:{eventData.companyId}");
                        return new OkObjectResult(responseData);
                    }
                }

                return new BadRequestObjectResult(response);
        }
    }
}
