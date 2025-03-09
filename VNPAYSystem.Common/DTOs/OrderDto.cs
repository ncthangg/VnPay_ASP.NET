namespace VNPAYSystem.Common.DTOs
{
    public class OrderDto
    {
        public string OrderCode { get; set; }

        public int UserId { get; set; }

        public decimal Amount { get; set; }

        public string Status { get; set; }

        public DateTime? CreatedAt { get; set; }
    }
}
