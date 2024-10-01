namespace MyCompanyWeb.Repositories
{
    public interface ISupplierRepository
    {
        Task<IEnumerable<Supplier>> GetSuppliers(string sTerm = "", string countryName = "");

        Task<Supplier> GetById(int id);
        bool Add(Supplier supplier);
        bool Delete(Supplier supplier);
        bool Edit(Supplier supplier);
        bool Save();
    }
}