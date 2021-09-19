using System;
using System.Collections.Generic;
using System.Text;

namespace IT.Netic.DispatchIt.Web.Backend.Common.Options
{
    /// <summary>
    /// A model which holds the default app options information used by contexts and startup file that need these common information for correct configuration
    /// </summary>
    public class AppOptions
    {
        public string CosmosHost { get; set; }
        public string CosmosUserName { get; set; }
        public string CosmosPassword { get; set; }

        public string SqlServerName { get; set; }
        public string SqlDatabaseName { get; set; }
        public string SqlUserName { get; set; }
        public string SqlPassword { get; set; }
    }
}
