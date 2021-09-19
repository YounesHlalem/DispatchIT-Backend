// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}
using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.Azure.EventGrid;
using IT.Netic.DispatchIt.Web.Backend.DataContracts.BaseDto;
using IT.Netic.DispatchIt.Web.Backend.Services.Interfaces;
using IT.Netic.DispatchIt.Web.Backend.Domain.Entities;
using Newtonsoft.Json;

namespace IT.Netic.DispatchIt.Web.Backend.Functions.Triggers
{
    public class EventTriggerDeliverResponse
    {
        private readonly IDeliveryrequestService _drService;

        public EventTriggerDeliverResponse(IDeliveryrequestService drService)
        {
            _drService = drService;
        }

        [FunctionName("EventTriggerDeliverResponse")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation($"C# trigger function begun");
            string response = string.Empty;

            string requestContent = await new StreamReader(req.Body).ReadToEndAsync();
            log.LogInformation($"Received events: {requestContent}");

            EventGridSubscriber eventGridSubscriber = new EventGridSubscriber();
            eventGridSubscriber.AddOrUpdateCustomEventMapping("Deliveries.Response", typeof(ResponseBaseDto));
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

                    return new OkObjectResult(responseData);
                }
                else if (eventGridEvent.Data is ResponseBaseDto)
                {
                    var eventData = (ResponseBaseDto)eventGridEvent.Data;
                    var result = _drService.UpdateDeliveryrequest(eventData.DeliveryId, eventData.MailResponse);
                    return new OkObjectResult(result);
                }
            }

            return new OkObjectResult(response);
        }
    }
}
