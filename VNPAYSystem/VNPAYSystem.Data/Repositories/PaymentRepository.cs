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

        public async Task<Payment> GetByOrderCode(string? orderCode)
        {
            return await _context.Set<Payment>()
                .Where(p => p.OrderCode == orderCode)
                .OrderByDescending(p => p.Id)
                .FirstOrDefaultAsync();
        }

        public async Task<Payment> GetByPaymentCode(string? paymentCode)
        {
            return await _context.Set<Payment>()
                .Where(p => p.PaymentCode == paymentCode)
                .OrderByDescending(p => p.Id)
                .FirstOrDefaultAsync();
        }


    }
}
