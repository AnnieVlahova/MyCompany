namespace MyCompanyWeb
{
    public interface IHomeRepository
    {

        Task<IEnumerable<Product>> GetProducts(string sTerm = "", string supplierName = "", int typeId = 0);
        Task<Product> GetProductById(int id);
        Task<ProductType> GetProductTypeById(int id);
        Task<Supplier> GetSupplierById(int id);

        Task<IEnumerable<ProductType>> Types();
        Task<IEnumerable<Supplier>> Suppliers();
        bool Add(Product product);
        bool Delete(Product product);
        bool Edit(Product product);
        bool Save();
    }

}