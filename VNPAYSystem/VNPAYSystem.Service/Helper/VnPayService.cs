using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Text;
using System.Web;
using VNPAYSystem.Common.DTOs;
using VNPAYSystem.Data;

namespace VNPAYSystem.Service.Helper
{
    public interface IVNPayService
    {
        string CreatePaymentURL(string orderCode, decimal amount);
        bool ValidateSignature(IQueryCollection queryParams);
    }
    public class VNPayService : IVNPayService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        public VNPayService(IConfiguration config)
        {
            _unitOfWork ??= new UnitOfWork();
            _config = config;
        }
        public string CreatePaymentURL(string orderCode , decimal amount)
        {
            var tick = DateTime.Now.Ticks.ToString();
            //Get Config Info
            string VNP_URL = _config["VNPaySettings:VNP_URL"];
            string VNP_TMNCODE = _config["VNPaySettings:VNP_TMNCODE"];
            string VNP_HASHSECRET = _config["VNPaySettings:VNP_HASHSECRET"];

            if (string.IsNullOrEmpty(VNP_TMNCODE) || string.IsNullOrEmpty(VNP_HASHSECRET))
            {
                return "Vui lòng cấu hình các tham số: vnp_TmnCode,vnp_HashSecret trong file web.config";
            }

            string VNP_AMOUNT = ((int)(amount * 100)).ToString();
            string VNP_COMMAND = _config["VNPaySettings:VNP_COMMAND"];
            string VNP_VERSION = _config["VNPaySettings:VNP_VERSION"];
            string VNP_CREATEDATE = DateTime.Now.ToString("yyyyMMddHHmmss");
            string VNP_CURRCODE = _config["VNPaySettings:VNP_CURRCODE"];
            string VNP_IPADDR = "127.0.0.1";
            string VNP_LOCATE = _config["VNPaySettings:VNP_LOCATE"];
            string VNP_ORDERINFO = "Thanh toan don hang " + orderCode;
            string VNP_ORDERTYPE = "order";
            string VNP_RETURNURL = _config["VNPaySettings:VNP_RETURNURL" ?? ""];
            string VNP_TXNREF = orderCode;


            var vnpay = new VnPayLibrary();
            vnpay.AddRequestData("vnp_Version", VNP_VERSION);
            vnpay.AddRequestData("vnp_Command", VNP_COMMAND);
            vnpay.AddRequestData("vnp_TmnCode", VNP_TMNCODE);
            vnpay.AddRequestData("vnp_Amount", VNP_AMOUNT); 
            vnpay.AddRequestData("vnp_CreateDate", VNP_CREATEDATE);
            vnpay.AddRequestData("vnp_CurrCode", VNP_CURRCODE);
            vnpay.AddRequestData("vnp_IpAddr", VNP_IPADDR);
            vnpay.AddRequestData("vnp_Locale", VNP_LOCATE);

            vnpay.AddRequestData("vnp_OrderInfo", VNP_ORDERINFO);
            vnpay.AddRequestData("vnp_OrderType", VNP_ORDERTYPE); 
            vnpay.AddRequestData("vnp_ReturnUrl", VNP_RETURNURL);

            vnpay.AddRequestData("vnp_TxnRef", VNP_TXNREF); 

            var paymentUrl = vnpay.CreateRequestUrl(VNP_URL, VNP_HASHSECRET);

            return paymentUrl;

        }
        public bool ValidateSignature(IQueryCollection queryParams)
        {
            if (!queryParams.ContainsKey("vnp_SecureHash")) return false;

            string vnp_SecureHash = queryParams["vnp_SecureHash"];

            var sortedParams = queryParams
                .Where(x => x.Key != "vnp_SecureHash" && !string.IsNullOrEmpty(x.Value))
                .OrderBy(x => x.Key, StringComparer.Ordinal)
                .Select(x => $"{x.Key}={x.Value}")
                .ToList();

            string rawData = string.Join("&", sortedParams);
            Console.WriteLine("RawData: " + rawData);

            string secureHash = Utils.HmacSHA512(_config["VNPaySettings:VNP_HASHSECRET"], rawData);
            Console.WriteLine("Generated SecureHash: " + secureHash);
            Console.WriteLine("Received SecureHash: " + vnp_SecureHash);

            return secureHash.Trim().ToUpper() == vnp_SecureHash.Trim().ToUpper();
        }

    }
}
