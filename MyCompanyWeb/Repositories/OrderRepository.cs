namespace MyCompanyWeb.Repositories
{

    public class OrderRepository
    {
        private readonly ApplicationDbContext _db;
        public OrderRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public void AddItem()
        {
            string userId = "";

        }
    }
}
