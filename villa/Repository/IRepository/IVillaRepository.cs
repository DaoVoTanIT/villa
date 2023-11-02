using System.Linq.Expressions;
using Villa_API.Model;

namespace Villa_API.Repository.IRepository
{
    public interface IVillaRepository: IRepository<Villa>
    {
        Task<Villa> UpdateAsync(Villa entity);
    }
}