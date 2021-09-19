using System;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using IT.Netic.DispatchIt.Web.Backend.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace IT.Netic.DispatchIt.Web.Backend.Functions.Triggers
{
    public class GetDeliveryrequestsByVat
    {
        private readonly IDeliveryrequestService _drService;
        private readonly IValidationService _validationService;

        public GetDeliveryrequestsByVat(IDeliveryrequestService drService, IValidationService validationService)
        {
            _drService = drService;
            _validationService = validationService;
        }

        [FunctionName("GetDeliveryrequestsByVat")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            string company = req.Query["companyVAT"];

            log.LogInformation("C# HTTP trigger function processed a request.");
            ClaimsPrincipal principal;
            if ((principal = await _validationService.ValidateTokenAsync(req.Headers["Authorization"])) == null && req.Headers["Source"] != "LogicApp")
            {
                return new BadRequestObjectResult("Forbidden");
            }
            else
            {
                var result = _drService.GetDeliveryrequestByVat(company);
                Type t = result.GetType();
                if (t.Equals(typeof(BadRequestObjectResult)))
                {
                    return null;
                };

                return result;
            }
        }
    }
}

