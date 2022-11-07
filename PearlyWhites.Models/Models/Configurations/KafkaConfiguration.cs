namespace PearlyWhites.Models.Models.Configurations
{
    public class KafkaConfiguration
    {
        public string BootstrapServers { get; set; }
        public int AutoOffsetReset { get; set; }
        public string GroupId { get; set; }
    }
}
