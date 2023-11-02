using System.Linq.Expressions;
using Villa_API.Model;

namespace Villa_API.Repository.IRepository
{
    public interface IVillaNumberRepository: IRepository<VillaNumber>
    {
        Task<VillaNumber> UpdateAsync(VillaNumber entity);
    }
}