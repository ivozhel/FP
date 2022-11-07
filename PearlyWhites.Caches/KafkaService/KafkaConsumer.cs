using Confluent.Kafka;
using Microsoft.Extensions.Options;
using PearlyWhites.Caches.KafkaService.GenericSerAndDeser;
using PearlyWhites.Models.Models.Configurations;

namespace PearlyWhites.Caches.KafkaService
{
    public abstract class KafkaConsumer<TKey, TValue>
    {
        private readonly ConsumerConfig _consumerConfig;
        private readonly IOptions<KafkaConfiguration> _kafkaSettings;
        protected readonly IConsumer<TKey, TValue> _consumer;
        public KafkaConsumer(IOptions<KafkaConfiguration> myJsonSettings)
        {
            _kafkaSettings = myJsonSettings;
            _consumerConfig = new ConsumerConfig()
            {
                BootstrapServers = _kafkaSettings.Value.BootstrapServers,
                AutoOffsetReset = (AutoOffsetReset)_kafkaSettings.Value.AutoOffsetReset,
                GroupId = _kafkaSettings.Value.GroupId,
            };

            _consumer = new ConsumerBuilder<TKey, TValue>(_consumerConfig).SetValueDeserializer(new DeserializeGen<TValue>())
                                                                             .SetKeyDeserializer(new DeserializeGen<TKey>()).Build();
            _consumer.Subscribe(typeof(TValue).Name);
        }
        public abstract Task Consume(CancellationToken cancellationToken);

    }
}
