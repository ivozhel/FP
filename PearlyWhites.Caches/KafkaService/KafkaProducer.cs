using Confluent.Kafka;
using Microsoft.Extensions.Options;
using PearlyWhites.Caches.KafkaService.GenericSerAndDeser;
using PearlyWhites.Models.Models.Configurations;

namespace PearlyWhites.Caches.KafkaService
{
    public class KafkaProducer<TKey, TValue>
    {
        private readonly ProducerConfig _producerConfig;
        private readonly IOptions<KafkaConfiguration> _kafkaSettings;
        public KafkaProducer(IOptions<KafkaConfiguration> myJsonSettings)
        {
            _kafkaSettings = myJsonSettings;
            _producerConfig = new ProducerConfig()
            {
                BootstrapServers = _kafkaSettings.Value.BootstrapServers
            };
        }

        public async Task Produce(TValue value, TKey key)
        {
            var producer = new ProducerBuilder<TKey, TValue>(_producerConfig).SetValueSerializer(new SerializerGen<TValue>())
                                                                            .SetKeySerializer(new SerializerGen<TKey>()).Build();
            var msg = new Message<TKey, TValue>()
            {
                Key = key,
                Value = value
            };
            await producer.ProduceAsync(typeof(TValue).Name, msg);

        }

    }
}
