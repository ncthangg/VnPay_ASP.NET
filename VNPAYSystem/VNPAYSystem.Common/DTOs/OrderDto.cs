namespace VNPAYSystem.Common.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string OrderCode { get; set; }

        public decimal Amount { get; set; }

        public string Status { get; set; }
    }
}
