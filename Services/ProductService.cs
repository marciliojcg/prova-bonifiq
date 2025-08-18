using ProvaPub.Models;
using ProvaPub.Repository;
using ProvaPub.Services.Interfaces;

namespace ProvaPub.Services
{
    public class ProductService : BaseService<Product>, IProductService
    {
        public ProductService(TestDbContext ctx) : base(ctx) { }

        protected override IQueryable<Product> GetBaseQuery() =>
            _ctx.Products.OrderBy(p => p.Id);
    }
}
