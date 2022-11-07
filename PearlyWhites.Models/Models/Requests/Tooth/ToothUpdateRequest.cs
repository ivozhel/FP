namespace PearlyWhites.Models.Models.Requests.Tooth
{
    public class ToothUpdateRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public List<int> TreatmentIds { get; set; } = new List<int>();
    }
}
