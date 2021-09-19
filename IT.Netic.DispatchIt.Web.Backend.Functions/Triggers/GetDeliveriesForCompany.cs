using System;
using System.Security.Claims;
using System.Threading.Tasks;
using IT.Netic.DispatchIt.Web.Backend.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace IT.Netic.DispatchIt.Web.Backend.Functions.Triggers
{
    public class GetDeliveriesForCompany
    {
        private readonly IDeliveryrequestService _drService;
        private readonly IValidationService _validationService;

        public GetDeliveriesForCompany(IDeliveryrequestService drService, IValidationService validationService)
        {
            _drService = drService;
            _validationService = validationService;
        }

        [FunctionName("GetDeliveriesForCompany")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "GetDeliveriesForCompany/{id}")] HttpRequest req,
            ILogger log, int id)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            ClaimsPrincipal principal;
            if ((principal = await _validationService.ValidateTokenAsync(req.Headers["Authorization"])) == null && req.Headers["Source"] != "LogicApp")
            {
                return new BadRequestObjectResult("Forbidden");
            }
            else
            {
                var result = _drService.GetDeliveryrequestByCId(id);
                return result;
            }
        }
    }
}

