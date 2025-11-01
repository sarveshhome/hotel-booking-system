using Kafka.Publishers.Configuration;
using Kafka.Publishers.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Kafka.Publishers.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddKafkaProducer(
            this IServiceCollection services,
            Action<KafkaSettings> configureOptions)
        {
            services.Configure(configureOptions);
            services.AddSingleton<IKafkaProducer, KafkaProducer>();
            return services;
        }
    }
}