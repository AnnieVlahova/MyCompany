using Microsoft.EntityFrameworkCore;

namespace MyCompanyWeb.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext _db;
        public CustomerRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IEnumerable<Customer>> GetCustomers(string sTerm = "", string cityName = "")
        {
            if (!String.IsNullOrEmpty(sTerm))
                sTerm = sTerm.ToLower();
            IEnumerable<Customer> customers = await (from customer in _db.Customers
                                                     where String.IsNullOrEmpty(sTerm) ||
                                                     (customer != null && customer.Name.ToLower().StartsWith(sTerm))
                                                     select new Customer
                                                     {
                                                         Id = customer.Id,
                                                         Name = customer.Name,
                                                         Country = customer.Country,
                                                         City = customer.City,
                                                         Email = customer.Email,
                                                         Director = customer.Director,
                                                         CustomerCode = customer.CustomerCode
                                                     }).ToListAsync();
            if(!String.IsNullOrEmpty(cityName))
            {
                customers = customers.Where(x => x.City.ToLower().StartsWith(cityName)).ToList();
            }
            return customers;
        }
        public async Task<Customer> GetById(int id)
        {
            Customer customer = await _db.Customers.FirstOrDefaultAsync(c => c.Id == id);
            return customer;

        }
    }
}
