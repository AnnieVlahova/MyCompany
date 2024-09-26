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
            IEnumerable<Customer> customers = await _db.Customers.Where(a => a.Name.ToLower().StartsWith(sTerm) || String.IsNullOrEmpty(sTerm)).ToListAsync();
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
        public bool Add(Customer customer)
        {
            _db.Add(customer);
            return Save();
        }
        public bool Delete(Customer customer)
        {
            _db.Remove(customer);
            return Save();
        }
        public bool Edit(Customer customer)
        {
            _db.Update(customer);
            return Save();
        }
        public bool Save()
        {
            var saved = _db.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
