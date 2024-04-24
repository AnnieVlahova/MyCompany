using Microsoft.EntityFrameworkCore;

namespace MyCompanyWeb.Repositories
{
    public class HomeRepository : IHomeRepository
    {
        private readonly ApplicationDbContext _db;
        public HomeRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<ProductType>> Types()
        {
            return await _db.ProductTypes.ToListAsync();
        }
        public async Task<IEnumerable<Product>> GetProducts(string sTerm = "", int typeId = 0)
        {
            if(!String.IsNullOrEmpty(sTerm))
                sTerm = sTerm.ToLower();
            IEnumerable<Product> products = await (from product in _db.Products
                            join type in _db.ProductTypes
                            on product.ProductTypeId equals type.Id
                            where string.IsNullOrWhiteSpace(sTerm) || 
                            (product != null && product.Name.ToLower().StartsWith(sTerm))
                            select new Product
                            {
                                Id = product.Id,
                                ProductTypeId = product.ProductTypeId,
                                ProductTypeName = type.Name,
                                Name = product.Name,
                                Description = product.Description,
                                Price = product.Price,
                                SerialNumber = product.SerialNumber,
                                InStock = product.InStock
                                
                            }).ToListAsync();
            if(typeId > 0)
            {
                products = products.Where(a => a.ProductTypeId == typeId).ToList();
            }
            return products;
        }
        public async Task<Product> GetById(int id)
        {
            Product product = await _db.Products.FirstOrDefaultAsync(c => c.Id == id);
            return product;

        }
    }
}
