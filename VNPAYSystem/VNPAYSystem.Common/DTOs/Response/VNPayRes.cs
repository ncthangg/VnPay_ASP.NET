namespace VNPAYSystem.Common.DTOs.Response
{

    public class VnPayRes
    {
        public string OrderId { get; set; }
        public string PaymentId { get; set; }
        public string TransactionId { get; set; }
        public string Token { get; set; }
        public string VnPayResponseCode { get; set; }

    }
}
