using Microsoft.EntityFrameworkCore;
using ProvaPub.Models;
using ProvaPub.Repository;
using ProvaPub.Services.Interfaces;

namespace ProvaPub.Services
{

    public abstract class BaseService<TEntity> : IBaseService<TEntity> where TEntity : class
    {
        protected readonly TestDbContext _ctx;
        protected abstract IQueryable<TEntity> GetBaseQuery();

        public BaseService(TestDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<PagedList<TEntity>> ListAsync(int page, int pageSize = 10)
        {
            var query = GetBaseQuery();
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedList<TEntity>
            {
                HasNext = await query.CountAsync() > page * pageSize,
                TotalCount = await query.CountAsync(),
                Items = items
            };
        }
    }
}