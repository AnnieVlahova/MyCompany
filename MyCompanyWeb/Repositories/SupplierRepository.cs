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

            IEnumerable<Supplier> suppliers = await _db.Suppliers
                .Where(s => String.IsNullOrEmpty(sTerm) || s.Name.ToLower().StartsWith(sTerm)).ToListAsync();

            if (!String.IsNullOrEmpty(countryName))
            {
                suppliers = suppliers.Where(x => x.Country.ToLower().StartsWith(countryName)).ToList();
            }
            return suppliers;
        }
    }
}
