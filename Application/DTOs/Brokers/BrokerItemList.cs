
namespace Application.DTOs.User
{
    public class BrokerItemList
    {
        public required string Slug { get; set; }

        public required string Name { get; set; }

        public bool IsVerified { get; set; }

        public string? ProfilePhoto { get; set; }

        public double AverageRating { get; set; }

        public int ReviewCount { get; set; }
    }
}
