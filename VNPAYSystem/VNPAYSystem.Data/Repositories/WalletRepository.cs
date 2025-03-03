using VNPAYSystem.Data.DBContext;
using VNPAYSystem.Data.Models;
using VNPAYSystem.Data.Repositories.Base;

namespace VNPAYSystem.Data.Repositories
{
    public class WalletRepository : GenericRepository<Wallet>
    {
        public WalletRepository()
        {
        }
        public WalletRepository(VNPAY_TestDBContext context) => _context = context;
    }
}
