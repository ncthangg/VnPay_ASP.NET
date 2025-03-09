using VNPAYSystem.Data.DBContext;
using VNPAYSystem.Data.Repositories;

namespace VNPAYSystem.Data
{
    public class UnitOfWork
    {
        private UserRepository _userRepository;
        private PaymentRepository _paymentRepository;
        private OrderRepository _orderRepository;

        private VNPAY_TestDBContext _dbContext;

        public UnitOfWork()
        {
            _dbContext ??= new VNPAY_TestDBContext();
        }

        public UserRepository UserRepository
        {
            get
            {
                return _userRepository ??= new Repositories.UserRepository(_dbContext);
            }
        }
        public PaymentRepository PaymentRepository
        {
            get
            {
                return _paymentRepository ??= new Repositories.PaymentRepository(_dbContext);
            }
        }
        public OrderRepository OrderRepository
        {
            get
            {
                return _orderRepository ??= new Repositories.OrderRepository(_dbContext);
            }
        }

    }
}

