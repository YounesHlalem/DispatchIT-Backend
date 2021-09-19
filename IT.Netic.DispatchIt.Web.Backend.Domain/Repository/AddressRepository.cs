using IT.Netic.DispatchIt.Web.Backend.Domain.Context;
using IT.Netic.DispatchIt.Web.Backend.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IT.Netic.DispatchIt.Web.Backend.Domain.Repository
{
    public class AddressRepository : BaseRepository<Address>, IAddressRepository
    {
        public AddressRepository(IDbContextCreator dbContextCreator) : base(dbContextCreator) { }

        public async Task<List<Address>> GetByCompany(int id)
        {
            List<Address> addresses = await GetAllAsync(x => x.CompanyId == id);

            return addresses;
        }

        public async Task<Address> GetById(int id)
        {
            var address = await Get(x => x.AddressId == id);

            return address;
        }
    }
}
