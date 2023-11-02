using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Villa_API.Data;
using Villa_API.Model;

namespace Villa_API.Repository.IRepository
{
    public class VillaNumberRepository : Repository<VillaNumber>, IVillaNumberRepository
    {
        private readonly ApplicationDbContext _db;
        public VillaNumberRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<VillaNumber> UpdateAsync(VillaNumber entity)
        {
            entity.UpdateDate = DateTime.Now;
            _db.VillasNumbers.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}