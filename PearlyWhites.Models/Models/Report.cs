namespace PearlyWhites.Models.Models
{
    public class Report
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Total { get; set; }
        public DateTime Date { get; set; }
    }
}
