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
    public class AddAddressToComp
    {
        private readonly ICompanyService _companyService;
        private readonly IValidationService _validationService;

        public AddAddressToComp(ICompanyService companyService, IValidationService validationService)
        {
            _companyService = companyService;
            _validationService = validationService;
        }

        [FunctionName("AddAddressToComp")]
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
                AddressBaseDto addressdata = JsonConvert.DeserializeObject<AddressBaseDto>(requestBody);

                var result = await _companyService.SendCreateAddressEvent(addressdata);

                log.LogInformation($"Completed:{addressdata.CompanyId}");
                return new OkObjectResult(result);
            }
        }
    }
}

