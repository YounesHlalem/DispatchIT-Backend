using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using IT.Netic.DispatchIt.Web.Backend.DataContracts.BaseDto;
using IT.Netic.DispatchIt.Web.Backend.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace IT.Netic.DispatchIt.Web.Backend.Functions.Triggers
{
    public class AddDeliveryrequest
    {
        private readonly IDeliveryrequestService _drService;
        private readonly IValidationService _validationService;
        private readonly ICompanyService _companyService;

        public AddDeliveryrequest(IDeliveryrequestService drService, IValidationService validationService, ICompanyService companyService)
        {
            _companyService = companyService;
            _drService = drService;
            _validationService = validationService;
        }

        [FunctionName("AddDeliveryrequest")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            ClaimsPrincipal principal;
            if ((principal = await _validationService.ValidateTokenAsync(req.Headers["Authorization"])) == null && req.Headers["Source"] != "LogicApp")
            {
                return new BadRequestObjectResult("Forbidden");
            }
            else
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                DeliveryrequestBaseDto deliveryData = JsonConvert.DeserializeObject<DeliveryrequestBaseDto>(requestBody);
                var result = await _drService.SendCreateDeliveryEvent(deliveryData);

                MessageDto messageData = new MessageDto();
                messageData.Title = "Delivery created!";
                messageData.Content = "Your deliveryrequest \"" + deliveryData.title + "\" has been created succesfully!";
                var res = _drService.CreateMessage(messageData);

                log.LogInformation($"Completed:{deliveryData.companyId}");
                return new OkObjectResult(result);
            }
        }
    }
}

