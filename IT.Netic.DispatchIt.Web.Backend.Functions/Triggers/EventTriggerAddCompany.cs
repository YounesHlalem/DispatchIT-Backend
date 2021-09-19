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
    public class EventTriggerAddCompany
    {
        private readonly ICompanyService _companyService;
        private readonly IValidationService _validationService;

        public EventTriggerAddCompany(ICompanyService companyService, IValidationService validationService)
        {
            _companyService = companyService;
            _validationService = validationService;
        }

        [FunctionName("EventTriggerAddCompany")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation($"C# trigger function begun");
            string response = string.Empty;
            
                string requestContent = await new StreamReader(req.Body).ReadToEndAsync();
                log.LogInformation($"Received events: {requestContent}");

                EventGridSubscriber eventGridSubscriber = new EventGridSubscriber();
                eventGridSubscriber.AddOrUpdateCustomEventMapping("Companies.Create", typeof(CompanyBaseDto));
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
                    else if (eventGridEvent.Data is CompanyBaseDto)
                    {
                        var eventData = (CompanyBaseDto)eventGridEvent.Data;
                        var responseData = _companyService.CreateCompany(eventData);
                        log.LogInformation($"Completed:{eventData.CompanyId}");
                        return new OkObjectResult(responseData);
                    }
                }

                return new OkObjectResult(response);
        }
    }
}
