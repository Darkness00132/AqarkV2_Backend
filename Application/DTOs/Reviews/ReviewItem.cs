
namespace Application.DTOs.Reviews
{
    public class ReviewItem
    {
        public required string ReviewerName { get; set; }

        public string? ReviewerProfilePhoto { get; set; }

        public required string Comment { get; set; }

        public required int Rating { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
    }
}
