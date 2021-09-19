using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IT.Netic.DispatchIt.Web.Backend.Services.Interfaces
{
    public interface IValidationService
    {
        public Task<ClaimsPrincipal> ValidateTokenAsync(string token);
    }
}
