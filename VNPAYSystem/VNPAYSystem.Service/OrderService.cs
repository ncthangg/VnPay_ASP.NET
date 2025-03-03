using VNPAYSystem.Common.DTOs;
using VNPAYSystem.Common.DTOs.Request;
using VNPAYSystem.Data;
using VNPAYSystem.Data.Models;

namespace VNPAYSystem.Service
{
    public interface IOrderService
    {
        Task<List<Order>> GetAll();
        Task<Order> GetById(int id);
        Task<Order> Create(OrderReq x);
        Task<Order> Update(int id, OrderDto x);
        Task<int> Delete(int id);
    }
    public class OrderService : IOrderService
    {
        private readonly UnitOfWork _unitOfWork;
        public OrderService()
        {
            _unitOfWork ??= new UnitOfWork();
        }
        public async Task<List<Order>> GetAll()
        {
            return await _unitOfWork.OrderRepository.GetAllAsync();
        }
        public async Task<Order> GetById(int id)
        {
            return await _unitOfWork.OrderRepository.GetByIdAsync(id);
        }
        public async Task<Order> Create(OrderReq x)
        {
            var order = new Order()
            {
                UserId = x.UserId,
                OrderCode = DateTime.Now.ToString("yyyyMMddHHmmss"),
                Amount = x.Amount,
                Status = "pending",
                CreatedAt = DateTime.Now,
            };
            return await _unitOfWork.OrderRepository.CreateAsync(order);
        }
        public async Task<Order> Update(int id, OrderDto x)
        {
            var order = new Order()
            {
                Id = x.Id,
                UserId = x.UserId,
                OrderCode = x.OrderCode,
                Amount = x.Amount,
                Status = x.Status,
                CreatedAt = DateTime.UtcNow,
            };
            return await _unitOfWork.OrderRepository.UpdateAsync(order);
        }
        public async Task<int> Delete(int id)
        {
            var order = await _unitOfWork.OrderRepository.GetByIdAsync(id);
            var result = await _unitOfWork.OrderRepository.RemoveAsync(order);
            if (!result)
            {
                return -1;
            }
            return 1;
        }
    }
}
