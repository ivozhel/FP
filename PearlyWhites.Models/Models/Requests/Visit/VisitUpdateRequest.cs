namespace PearlyWhites.Models.Models.Requests.Visit
{
    public class VisitUpdateRequest
    {
        public Guid Id { get; set; }
        public int PatientId { get; set; }
        public List<VisitTreatmentRequest> VisitTreatments { get; set; }
    }
}
