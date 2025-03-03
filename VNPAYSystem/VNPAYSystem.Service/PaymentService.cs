using Microsoft.AspNetCore.Http;
using System.Globalization;
using VNPAYSystem.Common.DTOs;
using VNPAYSystem.Common.DTOs.Response;
using VNPAYSystem.Data;
using VNPAYSystem.Data.Models;
using VNPAYSystem.Service.Helper;

namespace VNPAYSystem.Service
{
    public interface IPaymentService
    {
        Task<List<Payment>> GetAll();
        Task<Payment> GetById(int id);
        Task<PaymentRes> Create(int orderId);
        Task<int> Delete(int id);

        Task<bool> ProcessVNPayIPN(IQueryCollection queryParams);
    }
    public class PaymentService : IPaymentService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UnitOfWork _unitOfWork;
        private readonly IVNPayService _VNPayService;
        public PaymentService(IHttpContextAccessor httpContextAccessor, IVNPayService VNPayService)
        {
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork ??= new UnitOfWork();
            _VNPayService = VNPayService;
        }
        public async Task<List<Payment>> GetAll()
        {
            return await _unitOfWork.PaymentRepository.GetAllAsync();
        }
        public async Task<Payment> GetById(int id)
        {
            return await _unitOfWork.PaymentRepository.GetByIdAsync(id);
        }
        public async Task<PaymentRes> Create(int orderId)
        {
            var order = _unitOfWork.OrderRepository.GetById(orderId);
            var Payment = new Payment()
            {
                OrderId = order.Id,
                UserId = order.UserId,
                Amount = order.Amount,
                VnpayTransactionId = null,
                PaymentStatus = null,
                PaymentTime = null,
                BankCode = null,
                ResponseCode = null,
            };
            await _unitOfWork.PaymentRepository.CreateAsync(Payment);
            var paymentLink = _VNPayService.CreatePaymentURL(order.OrderCode, order.Amount);
            return new PaymentRes()
            {
                Payment = Payment,
                Link = paymentLink,
            };
        }
        public async Task<int> Delete(int id)
        {
            var Payment = await _unitOfWork.PaymentRepository.GetByIdAsync(id);
            var result = await _unitOfWork.PaymentRepository.RemoveAsync(Payment);
            if (!result)
            {
                return -1;
            }
            return 1;
        }


        public async Task<bool> ProcessVNPayIPN(IQueryCollection queryParams)
        {
            // **Bước 1: Kiểm tra chữ ký bảo mật**
            if (!_VNPayService.ValidateSignature(queryParams))
            {
                return false;
            }

            var vnp_ResponseCode = queryParams["vnp_ResponseCode"];
            var vnp_TransactionNo = queryParams["vnp_TransactionNo"];
            var vnp_TxnRef = queryParams["vnp_TxnRef"];
            var vnp_Amount = queryParams["vnp_Amount"];
            var vnp_BankCode = queryParams["vnp_BankCode"];
            var vnp_PayDate = queryParams["vnp_PayDate"];
            var vnp_SecureHash = queryParams["vnp_SecureHash"];


            // **Bước 2: Kiểm tra đơn hàng tồn tại**
            var payment = await _unitOfWork.PaymentRepository.GetByOrderCode(vnp_TxnRef);
            if (payment == null)
            {
                return false;
            }

            // **Bước 3: Cập nhật trạng thái thanh toán**
            if (vnp_ResponseCode == "00")  // "00" là giao dịch thành công
            {
                payment.VnpayTransactionId = vnp_TransactionNo;
                payment.PaymentStatus = "Success";
                payment.PaymentTime = DateTime.ParseExact(vnp_PayDate, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                payment.BankCode = vnp_BankCode;
                payment.ResponseCode = vnp_ResponseCode;
            }
            else
            {
                payment.PaymentStatus = "Failed";
                payment.ResponseCode = vnp_ResponseCode;
            }

            await _unitOfWork.PaymentRepository.UpdateAsync(payment);
            return true;
        }
    }
}
