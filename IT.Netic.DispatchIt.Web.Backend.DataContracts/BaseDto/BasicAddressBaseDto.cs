using System;
using System.Collections.Generic;
using System.Text;

namespace IT.Netic.DispatchIt.Web.Backend.DataContracts.BaseDto
{
    /// <summary>
    /// Models a Address Entity.
    /// Used as a basis with common properties for different address forms.
    /// </summary>
    public abstract class BasicAddressBaseDto
    {
        public string Country { get; set; }
        public string Zipcode { get; set; }
        public string City { get; set; }
        public string Streetname { get; set; }
        public string Number { get; set; }
        public string Addition { get; set; }

        public override string ToString()
        {
            return $"{Streetname} {Number}, {City} {Zipcode} in {Country}";
        }
    }
}
