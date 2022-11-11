namespace PearlyWhites.Models.Models.Requests.Visit
{
    public class VisitTreatmentRequest
    {
        public List<int> TreatmentId { get; set; }
        public Guid? ToothId { get; set; }
    }
}
