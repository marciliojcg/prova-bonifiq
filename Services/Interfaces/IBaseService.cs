using ProvaPub.Models;

namespace ProvaPub.Services.Interfaces
{
    public interface IBaseService<TEntity>
    {
        Task<PagedList<TEntity>> ListAsync(int page, int pageSize);
    }
}
