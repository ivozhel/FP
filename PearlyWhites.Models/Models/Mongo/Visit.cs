namespace PearlyWhites.Models.Models.Mongo
{
    public class Visit
    {
        public Guid Id { get; set; }
        public int PatientId { get; set; }
        public List<VisitTreatment> VisitTreatments { get; set; }
        public string ClinicName { get; set; }
        public decimal Paid { get; set; }
        public DateTime VisitDate { get; set; }

    }
}
