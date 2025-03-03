﻿namespace VNPAYSystem.Common.DTOs
{
    public class PaymentDto
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public int UserId { get; set; }

        public string VnpayTransactionId { get; set; }

        public decimal Amount { get; set; }

        public string PaymentStatus { get; set; }

        public DateTime? PaymentTime { get; set; }

        public string BankCode { get; set; }

        public string ResponseCode { get; set; }
    }
}
