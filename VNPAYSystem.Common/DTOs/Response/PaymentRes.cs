using VNPAYSystem.Data.Models;

namespace VNPAYSystem.Common.DTOs.Response
{
    public class PaymentRes
    {
        public Payment Payment { get; set; }
        public string Link { get; set; }

    }
}
