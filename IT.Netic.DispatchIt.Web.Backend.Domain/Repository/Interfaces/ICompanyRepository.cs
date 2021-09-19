using IT.Netic.DispatchIt.Web.Backend.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IT.Netic.DispatchIt.Web.Backend.Domain.Repository
{
    public interface ICompanyRepository : IBaseRepository<Company>
    {
        Task<Company> GetById(int id);
        Task<Company> GetByVat(string vat);
        Task<List<Company>> GetByOwner(string ownerId);
    }
}
