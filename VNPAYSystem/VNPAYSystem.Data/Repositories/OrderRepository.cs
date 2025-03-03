using VNPAYSystem.Data.DBContext;
using VNPAYSystem.Data.Models;
using VNPAYSystem.Data.Repositories.Base;

namespace VNPAYSystem.Data.Repositories
{
    public class OrderRepository : GenericRepository<Order>
    {
        public OrderRepository()
        {
        }
        public OrderRepository(VNPAY_TestDBContext context) => _context = context;
    }
}
