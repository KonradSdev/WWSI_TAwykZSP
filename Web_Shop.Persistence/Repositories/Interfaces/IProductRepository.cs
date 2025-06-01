using WWSI_Shop.Persistence.MySQL.Model;

namespace Web_Shop.Persistence.Repositories.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<bool> NameExistsAsync(string name);
        Task<bool> IsNameEditAllowedAsync(string name, ulong id);
        Task<Product?> GetByNameAsync(string name);
        Task<bool> SkuExistsAsync(string sku);
    }
}
