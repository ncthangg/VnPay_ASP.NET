using Microsoft.EntityFrameworkCore;
using VNPAYSystem.Data.DBContext;
using VNPAYSystem.Data.Models;
using VNPAYSystem.Data.Repositories.Base;

namespace VNPAYSystem.Data.Repositories
{
    public class UserRepository : GenericRepository<User>
    {
        public UserRepository()
        {
        }
        public UserRepository(VNPAY_TestDBContext context) => _context = context;

        public async Task<User> Login(string username, string password)
        {
            return await _context.Set<User>().FirstOrDefaultAsync(u => u.Email == username && u.Password == password);
        }
    }
}
