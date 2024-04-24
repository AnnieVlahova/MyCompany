namespace MyCompanyWeb.Repositories
{
    public interface ISupplierRepository
    {
        Task<IEnumerable<Supplier>> GetSuppliers(string sTerm = "", string countryName = "");
    }
}