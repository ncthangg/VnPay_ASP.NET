namespace VNPAYSystem.Common.DTOs
{
    public class WalletDto
    {
        public int UserId { get; set; }

        public decimal Balance { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
