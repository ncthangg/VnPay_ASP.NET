using VNPAYSystem.Common.DTOs;
using VNPAYSystem.Common.DTOs.Request;
using VNPAYSystem.Data;
using VNPAYSystem.Data.Models;

namespace VNPAYSystem.Service
{
    public interface IOrderService
    {
        Task<List<Order>> GetAll();
        Task<Order> GetById(string code);
        Task<Order> Create(OrderReq x);
        Task<int> Delete(string code);
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
        public async Task<Order> GetById(string code)
        {
            return await _unitOfWork.OrderRepository.GetByIdAsync(code);
        }
        public async Task<Order> Create(OrderReq x)
        {
            var tick = DateTime.Now.Ticks.ToString();
            var order = new Order()
            {
                OrderCode = $"{x.UserId}{tick}",
                UserId = x.UserId,
                Amount = x.Amount,
                Status = "Pending",
                CreatedAt = DateTime.Now,
            };
            return await _unitOfWork.OrderRepository.CreateAsync(order);
        }
        public async Task<int> Delete(string code)
        {
            var order = await _unitOfWork.OrderRepository.GetByIdAsync(code);
            var result = await _unitOfWork.OrderRepository.RemoveAsync(order);
            if (!result)
            {
                return -1;
            }
            return 1;
        }
    }
}
