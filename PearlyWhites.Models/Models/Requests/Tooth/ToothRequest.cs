namespace PearlyWhites.Models.Models.Requests.Tooth
{
    public record ToothRequest
    {
        public string Name { get; set; }
        public string Position { get; set; }
        public List<int> TreatmentIds { get; set; } = new List<int>();
    }
}
