using PearlyWhites.Models.Models.Requests.Tooth;

namespace PearlyWhites.Models.Models.Requests.Patient
{
    public record PatientRequest
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public List<ToothRequest> Teeth { get; set; } = new List<ToothRequest>();
    }
}
