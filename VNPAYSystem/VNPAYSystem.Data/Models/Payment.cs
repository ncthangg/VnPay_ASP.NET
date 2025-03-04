﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace VNPAYSystem.Data.Models;

public partial class Payment
{
    public int Id { get; set; }

    public string OrderCode { get; set; }

    public int UserId { get; set; }

    public int? PaymentAttempt { get; set; }

    public string PaymentCode { get; set; }

    public string VnpayTransactionId { get; set; }

    public decimal Amount { get; set; }

    public string PaymentStatus { get; set; }

    public DateTime? PaymentTime { get; set; }

    public string BankCode { get; set; }

    public string ResponseCode { get; set; }

    public virtual Order OrderCodeNavigation { get; set; }

    public virtual User User { get; set; }
}