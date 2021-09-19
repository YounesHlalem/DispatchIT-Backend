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
    public class EditCompany
    {
        private readonly ICompanyService _companyService;
        private readonly IValidationService _validationService;

        public EditCompany(ICompanyService companyService, IValidationService validationService)
        {
            _companyService = companyService;
            _validationService = validationService;
        }

        [FunctionName("EditCompany")]
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
                CompanyBaseDto companyData = JsonConvert.DeserializeObject<CompanyBaseDto>(requestBody);
                var responseData = _companyService.PutCompany(companyData);
                log.LogInformation($"Completed:{companyData.CompanyId}");
                return new OkObjectResult(responseData);
            }
        }
    }
}
