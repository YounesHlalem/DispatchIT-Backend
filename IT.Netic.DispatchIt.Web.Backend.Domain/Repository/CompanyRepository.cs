using IT.Netic.DispatchIt.Web.Backend.Domain.Context;
using IT.Netic.DispatchIt.Web.Backend.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IT.Netic.DispatchIt.Web.Backend.Domain.Repository
{
    public class CompanyRepository : BaseRepository<Company>, ICompanyRepository
    {
        public CompanyRepository(IDbContextCreator dbContextCreator) : base(dbContextCreator) { }

        public async Task<Company> GetById(int id)
        {
            var information = await Get(x => x.CompanyId == id);

            return information;
        }

        public async Task<Company> GetByVat(string vat)
        {
            var information = await Get(x => x.VatNr == vat);

            return information;
        }

        public async Task<List<Company>> GetByOwner(string ownerId)
        {
            List<Company> information = await GetAllAsync(x => x.Owner == ownerId);

            return information;
        }
    }
}
