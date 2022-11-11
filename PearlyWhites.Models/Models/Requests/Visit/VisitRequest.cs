namespace PearlyWhites.Models.Models.Requests.Visit
{
    public class VisitRequest
    {
        public int PatientId { get; set; }
        public List<VisitTreatmentRequest> VisitTreatments { get; set; }
    }
}
