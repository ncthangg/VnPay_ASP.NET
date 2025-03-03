using Microsoft.EntityFrameworkCore;
using VNPAYSystem.Data.DBContext;
using VNPAYSystem.Data.Models;
using VNPAYSystem.Data.Repositories.Base;

namespace VNPAYSystem.Data.Repositories
{
    public class PaymentRepository : GenericRepository<Payment>
    {
        public PaymentRepository()
        {
        }
        public PaymentRepository(VNPAY_TestDBContext context) => _context = context;

        public async Task<Payment> GetByOrderCode(string orderCode)
        {
            return await _context.Set<Payment>()
                        .Include(p => p.Order)
                        .Where(p => p.Order.OrderCode == orderCode)
                        .OrderByDescending(p => p.Id)
                        .FirstOrDefaultAsync();
        }
    }
}
