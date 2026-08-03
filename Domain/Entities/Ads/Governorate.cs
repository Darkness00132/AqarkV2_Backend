namespace Domain.Entities.AdEntities
{
    public class Governorate
    {
        public int Id { get; set; }
        public string NameEn { get; set; } = string.Empty;
        public string NameAr { get; set; } = string.Empty;
        public List<City> Cities { get; set; } = [];
    }

    public class City
    {
        public int Id { get; set; }

        public string NameEn { get; set; } = string.Empty;
        public string NameAr { get; set; } = string.Empty;
    }
}
