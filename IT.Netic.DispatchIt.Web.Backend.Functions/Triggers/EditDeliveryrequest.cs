using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using IT.Netic.DispatchIt.Web.Backend.Services.Interfaces;
using System.Security.Claims;
using IT.Netic.DispatchIt.Web.Backend.DataContracts.BaseDto;

namespace IT.Netic.DispatchIt.Web.Backend.Functions.Triggers
{
    public class EditDeliveryrequest
    {
        private readonly IDeliveryrequestService _drService;
        private readonly IValidationService _validationService;

        public EditDeliveryrequest(IDeliveryrequestService drService, IValidationService validationService)
        {
            _drService = drService;
            _validationService = validationService;
        }

        [FunctionName("EditDeliveryrequest")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = null)] HttpRequest req,
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
                var responseData = _drService.PutDeliveryrequest(deliveryData);
                log.LogInformation($"Completed:{deliveryData.companyId}");
                return new OkObjectResult(responseData);
            }
        }
    }
}
