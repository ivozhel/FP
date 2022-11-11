namespace PearlyWhites.Models.Models.Mongo
{
    public class VisitTreatment
    {
        public List<Treatment> Treatments { get; set; }
        public Tooth Tooth { get; set; }
    }
}
