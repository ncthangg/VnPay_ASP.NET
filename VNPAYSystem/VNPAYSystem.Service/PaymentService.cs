using Microsoft.AspNetCore.Http;
using System.Globalization;
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
        Task<PaymentRes> Create(string orderCode);
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
        public async Task<PaymentRes> Create(string orderCode)
        {
            var order = await _unitOfWork.OrderRepository.GetByIdAsync(orderCode);
            if( order.Status.Equals("Success") || order.Status.Equals("Fail") )
            {
                throw new Exception("Đơn hàng đã thanh toán hoặc đã bị hủy");
            }

            var payment = await _unitOfWork.PaymentRepository.GetByOrderCode(orderCode);
            string paymentLink = "";
            if (payment != null)
            {
                // Tăng giá trị lên 1 rồi chuyển lại thành string
                var newPaymentAttempt = payment.PaymentAttempt + 1;

                string newPaymentCode = $"{orderCode}{newPaymentAttempt}";

                var Payment = new Payment()
                {
                    OrderCode = order.OrderCode,
                    UserId = order.UserId,
                    Amount = order.Amount,
                    PaymentCode = newPaymentCode,
                    PaymentAttempt= newPaymentAttempt,
                    VnpayTransactionId = null,
                    PaymentStatus = null,
                    PaymentTime = null,
                    BankCode = null,
                    ResponseCode = null,
                };
                await _unitOfWork.PaymentRepository.CreateAsync(Payment);
                paymentLink = await _VNPayService.CreatePaymentURL(newPaymentCode, order.Amount);
                return new PaymentRes()
                {
                    Payment = Payment,
                    Link = paymentLink,
                };
            }

            var NewPayment = new Payment()
            {
                OrderCode = order.OrderCode,
                UserId = order.UserId,
                Amount = order.Amount,
                PaymentCode = null,
                PaymentStatus = null,
                PaymentTime = null,
                BankCode = null,
                ResponseCode = null,
            };
            await _unitOfWork.PaymentRepository.CreateAsync(NewPayment);
            paymentLink = await _VNPayService.CreatePaymentURL(order.OrderCode, order.Amount);
            return new PaymentRes()
            {
                Payment = NewPayment,
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


        public async Task<bool> ProcessVNPayIPN(IQueryCollection collections)
        {
            var result = _VNPayService.PaymentExecute(collections);
            // **Bước 1: Kiểm tra chữ ký bảo mật**
            if (result.Success == false)
            {
                return false;
            }

            var vnp_ResponseCode = collections["vnp_ResponseCode"];
            var vnp_TransactionNo = collections["vnp_TransactionNo"];
            var vnp_TxnRef = collections["vnp_TxnRef"].ToString();
            var vnp_Amount = collections["vnp_Amount"];
            var vnp_BankCode = collections["vnp_BankCode"];
            var vnp_PayDate = collections["vnp_PayDate"];
            var vnp_SecureHash = collections["vnp_SecureHash"];


            // **Bước 2: Kiểm tra đơn hàng tồn tại**
            var payment = await _unitOfWork.PaymentRepository.GetByPaymentCode(result.OrderId);
            if (payment == null)
            {
                return false;
            }

            var order = await _unitOfWork.OrderRepository.GetByIdAsync(payment.OrderCode);
            if (order == null)
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

                order.Status = "Success";
            }
            else
            {
                payment.PaymentStatus = "Failed";
                payment.ResponseCode = vnp_ResponseCode;

                order.Status = "Fail";
            }
            


            await _unitOfWork.PaymentRepository.UpdateAsync(payment);
            await _unitOfWork.OrderRepository.UpdateAsync(order);
            return true;
        }

    }
}
