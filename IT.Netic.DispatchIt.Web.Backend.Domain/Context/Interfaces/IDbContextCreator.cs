using System;
using System.Collections.Generic;
using System.Text;

namespace IT.Netic.DispatchIt.Web.Backend.Domain.Context
{
    public interface IDbContextCreator
    {
        IDispatchItDbContext CreateDispatchItContext();
    }
}
