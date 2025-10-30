namespace Kafka.Publishers.Configuration
{
    public class KafkaSettings
    {
        public string BootstrapServers { get; set; } = string.Empty;
        public string ClientId { get; set; } = string.Empty;
    }
}   