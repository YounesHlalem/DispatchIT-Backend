using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using IT.Netic.DispatchIt.Web.Backend.Common.Options;

namespace IT.Netic.DispatchIt.Web.Backend.Domain.Context
{
    /// <summary>
    /// A seperate class to invoke the creation using the dispatchItContext.
    /// </summary>
    public class DbContextCreator : IDbContextCreator
    {
        private readonly IOptions<AppOptions> _appOptions;

        public DbContextCreator(IOptions<AppOptions> appOptions) 
        {
            _appOptions = appOptions;
        }

        public IDispatchItDbContext CreateDispatchItContext()
        {
            var dispatchItContext = new DispatchItDbContext(new DbContextOptions<DispatchItDbContext>(), _appOptions);
            return dispatchItContext;
        }
    }
}
