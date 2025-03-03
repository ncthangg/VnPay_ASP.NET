using VNPAYSystem.Common.DTOs;
using VNPAYSystem.Data;
using VNPAYSystem.Data.Models;

namespace VNPAYSystem.Service
{
    public interface IWalletService
    {
        Task<List<Wallet>> GetAll();
        Task<Wallet> GetById(int id);
        Task<Wallet> Create(WalletDto x);
        Task<Wallet> Update(int id, WalletDto x);
        Task<int> Delete(int id);
    }
    public class WalletService : IWalletService
    {
        private readonly UnitOfWork _unitOfWork;
        public WalletService()
        {
            _unitOfWork ??= new UnitOfWork();
        }
        public async Task<List<Wallet>> GetAll()
        {
            return await _unitOfWork.WalletRepository.GetAllAsync();
        }
        public async Task<Wallet> GetById(int id)
        {
            return await _unitOfWork.WalletRepository.GetByIdAsync(id);
        }
        public async Task<Wallet> Create(WalletDto x)
        {
            var Wallet = new Wallet()
            {
                UserId = x.UserId,
                Balance = x.Balance,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };
            return await _unitOfWork.WalletRepository.CreateAsync(Wallet);
        }
        public async Task<Wallet> Update(int id, WalletDto x)
        {
            var Wallet = new Wallet()
            {
                UserId = x.UserId,
                Balance = x.Balance,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };
            return await _unitOfWork.WalletRepository.UpdateAsync(Wallet);
        }
        public async Task<int> Delete(int id)
        {
            var Wallet = await _unitOfWork.WalletRepository.GetByIdAsync(id);
            var result = await _unitOfWork.WalletRepository.RemoveAsync(Wallet);
            if (!result)
            {
                return -1;
            }
            return 1;
        }
    }
}
