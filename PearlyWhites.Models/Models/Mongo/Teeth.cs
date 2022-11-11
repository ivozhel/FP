namespace PearlyWhites.Models.Models.Mongo
{
    public class Teeth
    {
        public Guid Id { get; set; }
        public List<Tooth> TeethList { get; set; }
        public int PatientId { get; set; }
    }
}
