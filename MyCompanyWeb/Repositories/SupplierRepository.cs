using Microsoft.EntityFrameworkCore;

namespace MyCompanyWeb.Repositories
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly ApplicationDbContext _db;
        public SupplierRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IEnumerable<Supplier>> GetSuppliers(string sTerm = "", string countryName = "")
        {
            if (!String.IsNullOrEmpty(sTerm))
                sTerm = sTerm.ToLower();
            IEnumerable<Supplier> suppliers = await (from supplier in _db.Suppliers
                                                     where String.IsNullOrEmpty(sTerm) ||
                                                     (supplier != null && supplier.Name.ToLower().StartsWith(sTerm))
                                                     select new Supplier
                                                     {
                                                         Id = supplier.Id,
                                                         Name = supplier.Name,
                                                         Country = supplier.Country,
                                                         City = supplier.City,
                                                         Email = supplier.Email
                                                     }).ToListAsync();
            if (!String.IsNullOrEmpty(countryName))
            {
                suppliers = suppliers.Where(x => x.Country.ToLower().StartsWith(countryName)).ToList();
            }
            return suppliers;
        }
    }
}
