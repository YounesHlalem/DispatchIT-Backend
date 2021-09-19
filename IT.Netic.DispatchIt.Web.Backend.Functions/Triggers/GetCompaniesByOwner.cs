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

namespace IT.Netic.DispatchIt.Web.Backend.Functions
{
    public class GetCompaniesByOwner
    {
        private readonly ICompanyService _companyService;
        private readonly IValidationService _validationService;

        public GetCompaniesByOwner(ICompanyService companyService, IValidationService validationService)
        {
            _companyService = companyService;
            _validationService = validationService;
        }

        [FunctionName("GetCompaniesByOwner")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            string owner = req.Query["owner"];
            log.LogInformation("C# HTTP trigger function processed a request.");

            ClaimsPrincipal principal;
            if ((principal = await _validationService.ValidateTokenAsync(req.Headers["Authorization"])) == null && req.Headers["Source"] != "LogicApp")
            {
                return new BadRequestObjectResult("Forbidden");
            }
            else
            {
                var result = await _companyService.GetCompanyByOwner(owner);
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

