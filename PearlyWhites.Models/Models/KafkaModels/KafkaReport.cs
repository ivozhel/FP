using MessagePack;

namespace PearlyWhites.Models.Models.KafkaModels
{
    [MessagePackObject]
    public class KafkaReport
    {
        [Key(0)]
        public Guid Id { get; set; }
        [Key(1)]
        public string Name { get; set; }
        [Key(2)]
        public IEnumerable<int> DailyTreatmentIds { get; set; }
        [Key(3)]
        public DateTime Date { get; set; }
    }
}
