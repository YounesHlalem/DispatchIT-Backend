using System;
using System.Collections.Generic;
using System.Text;

namespace IT.Netic.DispatchIt.Web.Backend.DataContracts.BaseDto
{
    /// <summary>
    /// Models a Company Entity.
    /// </summary>
    public class CompanyBaseDto
    {
        public int CompanyId { get; set; }
        public string VatNr { get; set; }
        public string Name { get; set; }
        public string PhoneNr { get; set; }
        public string Email { get; set; }
        public string Owner { get; set; }
        public List<AddressBaseDto> addresses { get; set; }
    }
}
