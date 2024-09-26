namespace MyCompanyWeb.Repositories
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> GetCustomers(string sTerm = "", string cityName = "");
        Task<Customer> GetById(int id);
        bool Add(Customer customer);
        bool Delete(Customer customer);
        bool Edit(Customer customer);
        bool Save();
    }
}