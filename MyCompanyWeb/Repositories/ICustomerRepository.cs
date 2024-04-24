namespace MyCompanyWeb.Repositories
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> GetCustomers(string sTerm = "", string cityName = "");
        Task<Customer> GetById(int id);
    }
}