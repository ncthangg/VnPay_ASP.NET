using Microsoft.EntityFrameworkCore;
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

        //public async Task<Order> GetByOrderCode(string? orderCode)
        //{
        //    return await _context.Set<Order>()
        //        .Where(p => p.OrderCode == orderCode)
        //        .OrderByDescending(p => p.Id)
        //        .FirstOrDefaultAsync();
        //}
    }
}
