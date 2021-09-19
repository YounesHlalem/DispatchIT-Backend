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

namespace IT.Netic.DispatchIt.Web.Backend.Functions.Triggers
{
    public class GetMessages
    {
        private readonly IDeliveryrequestService _drService;
        private readonly IValidationService _validationService;

        public GetMessages(IDeliveryrequestService drService, IValidationService validationService)
        {
            _drService = drService;
            _validationService = validationService;
        }

        [FunctionName("GetMessages")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "GetMessages/{user}")] HttpRequest req,
            ILogger log, string user)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            ClaimsPrincipal principal;
            if ((principal = await _validationService.ValidateTokenAsync(req.Headers["Authorization"])) == null && req.Headers["Source"] != "LogicApp")
            {
                return new BadRequestObjectResult("Forbidden");
            }
            else
            {
                var result = _drService.GetMessagesForUser(user);
                return result;
            }
        }
    }
}
