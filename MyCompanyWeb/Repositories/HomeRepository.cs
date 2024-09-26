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
        public async Task<IEnumerable<Supplier>> Suppliers()
        {
            return await _db.Suppliers.ToListAsync();
        }
        public async Task<IEnumerable<Product>> GetProducts(string sTerm = "", string supplierName = "", int typeId = 0)
        {
            if (!String.IsNullOrEmpty(sTerm))
                sTerm = sTerm.ToLower();
            IEnumerable<Product> products = await _db.Products.Include(s => s.Supplier).Include(p => p.ProductType).Where(p => String.IsNullOrEmpty(sTerm) || p.Name.ToLower().StartsWith(sTerm)).ToListAsync();

            if (!String.IsNullOrEmpty(supplierName))
            {
                products = products.Where(s => s.Supplier.Name.StartsWith(supplierName) || String.IsNullOrEmpty(supplierName)).ToList();
            }
            if (typeId > 0)
            {
                products = products.Where(a => a.ProductTypeId == typeId).ToList();
            }

            return products;

        }
        public async Task<Product> GetProductById(int id)
        {
            Product product = await _db.Products.Include(p => p.ProductType).Include(s => s.Supplier).FirstOrDefaultAsync(c => c.Id == id);
            return product;

        }
        public async Task<ProductType> GetProductTypeById(int id)
        {
            ProductType productType = await _db.ProductTypes.FirstOrDefaultAsync(c => c.Id == id);
            return productType;

        }
        public async Task<Supplier> GetSupplierById(int id)
        {
            Supplier supplier = await _db.Suppliers.FirstOrDefaultAsync(c => c.Id == id);
            return supplier;

        }
        public bool Add(Product product)
        {
            _db.Add(product);
            return Save();
        }
        public bool Delete(Product product)
        {
            _db.Remove(product);
            return Save();
        }
        public bool Edit(Product product)
        {
            _db.Update(product);
            return Save();
        }
        public bool Save()
        {
            var saved = _db.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
