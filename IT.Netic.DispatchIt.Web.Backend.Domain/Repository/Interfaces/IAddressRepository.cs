using IT.Netic.DispatchIt.Web.Backend.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IT.Netic.DispatchIt.Web.Backend.Domain.Repository
{
    public interface IAddressRepository : IBaseRepository<Address>
    {
        Task<List<Address>> GetByCompany(int id);
        Task<Address> GetById(int id);
    }
}
