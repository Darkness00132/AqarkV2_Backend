namespace Domain.Entities.AdEntities
{
    public class Image
    {
        public int Id { get; set; }

        public required string Url { get; set; }

        public Guid AdId { get; set; }
    }
}
