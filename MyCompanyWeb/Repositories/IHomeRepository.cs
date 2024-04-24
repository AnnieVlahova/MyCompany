namespace MyCompanyWeb
{
    public interface IHomeRepository
    {
        Task<IEnumerable<Product>> GetProducts(string sTerm = "", int typeId = 0);
        Task<IEnumerable<ProductType>> Types();

        Task<Product> GetById(int id);
    }
}