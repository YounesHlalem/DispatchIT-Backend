using IT.Netic.DispatchIt.Web.Backend.DataContracts.BaseDto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IT.Netic.DispatchIt.Web.Backend.Services.Interfaces
{
    public interface ICompanyService
    {
        Task<IActionResult> CreateCompany(CompanyBaseDto company);
        Task<IActionResult> GetCompanyByOwner(string owner);
        Task<IActionResult> CreateAddress(AddressBaseDto addressdata);
        Task<IActionResult> SendCreateCompanyEvent(CompanyBaseDto company);
        Task<IActionResult> SendCreateAddressEvent(AddressBaseDto address);
        Task<IActionResult> GetCompany(int id);
        Task<IActionResult> PutCompany(CompanyBaseDto company);
        Task<IActionResult> DeleteCompany(int company);
    }
}
