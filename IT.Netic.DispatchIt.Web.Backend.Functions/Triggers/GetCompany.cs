using IT.Netic.DispatchIt.Web.Backend.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IT.Netic.DispatchIt.Web.Backend.Functions.Triggers
{
    public class GetCompany
    {
        private readonly ICompanyService _companyService;
        private readonly IValidationService _validationService;

        public GetCompany(ICompanyService companyService, IValidationService validationService)
        {
            _companyService = companyService;
            _validationService = validationService;
        }

        [FunctionName("GetCompany")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "GetCompany/{id}")] HttpRequest req,
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
                var result = await _companyService.GetCompany(id);
                return  result;
            }
        }
    }
}
