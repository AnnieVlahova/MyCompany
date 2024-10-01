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
        public async Task<Supplier> GetById(int id)
        {
            Supplier supplier = await _db.Suppliers.FirstOrDefaultAsync(x => x.Id == id);
            return supplier;
        }
        public bool Add(Supplier supplier)
        {
            _db.Add(supplier);
            return Save();
        }
        public bool Delete(Supplier supplier)
        {
            _db.Remove(supplier);
            return Save();
        }
        public bool Edit(Supplier supplier)
        {
            _db.Update(supplier);
            return Save();
        }
        public bool Save()
        {
            var saved = _db.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
