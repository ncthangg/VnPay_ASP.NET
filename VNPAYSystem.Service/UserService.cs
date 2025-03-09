using VNPAYSystem.Data;
using VNPAYSystem.Data.Models;

namespace VNPAYSystem.Service
{
    public interface IUserService
    {
        Task<User> Login(string username, string password);
    }
    public class UserService : IUserService
    {
        private readonly UnitOfWork _unitOfWork;
        public UserService()
        {
            _unitOfWork ??= new UnitOfWork();
        }
        public Task<User> Login(string username, string password)
        {
            return _unitOfWork.UserRepository.Login(username, password);
        }
    }
}
