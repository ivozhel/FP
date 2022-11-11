namespace PearlyWhites.Models.Models.Requests.Visit
{
    public record TreatmentRequest
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }

    }
}
