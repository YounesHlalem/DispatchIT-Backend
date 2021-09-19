using System;
using System.Collections.Generic;
using System.Text;

namespace IT.Netic.DispatchIt.Web.Backend.Domain.Entities
{
    /// <summary>
    /// Models a Address Entity.
    /// Used as a basis with common properties for different address forms.
    /// </summary>
    public abstract class BasicAddress
    {
        public string Country { get; set; }
        public string Zipcode { get; set; }
        public string City { get; set; }
        public string Streetname { get; set; }
        public string Number { get; set; }
        public string Addition { get; set; }
    }
}
