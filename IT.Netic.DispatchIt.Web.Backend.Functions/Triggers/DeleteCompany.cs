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
    public class DeleteCompany
    {
        private readonly IDeliveryrequestService _drService;
        private readonly ICompanyService _companyService;
        private readonly IValidationService _validationService;

        public DeleteCompany(ICompanyService companyService, IValidationService validationService, IDeliveryrequestService drService)
        {
            _drService = drService;
            _companyService = companyService;
            _validationService = validationService;
        }

        [FunctionName("DeleteCompany")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "DeleteCompany/{CompanyId}")] HttpRequest req,
            ILogger log, int CompanyId)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            ClaimsPrincipal principal;
            if ((principal = await _validationService.ValidateTokenAsync(req.Headers["Authorization"])) == null && req.Headers["Source"] != "LogicApp")
            {
                return new BadRequestObjectResult("Forbidden");
            }
            else
            {
                var responseData = _companyService.DeleteCompany(CompanyId);
                var result = _drService.DeleteDeliveryrequestByCId(CompanyId);

                log.LogInformation($"Deleted company with id: {CompanyId}");
                return new OkObjectResult(responseData);
            }
        }
    }
}
