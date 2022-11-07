namespace PearlyWhites.Models.Models.Requests
{
    public record TreatmentRequest
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }

    }
}
