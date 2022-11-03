namespace PearlyWhites.Models.Models
{
    public record Patient
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? Age { get; set; }
        public List<Tooth> Teeth { get; set; } = new List<Tooth>();
    }
}
