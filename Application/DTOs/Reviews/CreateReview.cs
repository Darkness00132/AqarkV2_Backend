namespace Application.DTOs.Reviews
{
    public class CreateReview
    {
        public required int Rating { get; set; }
        public required string Comment { get; set; }
    }
}
