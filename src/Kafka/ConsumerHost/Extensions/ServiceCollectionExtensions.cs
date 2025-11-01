// src/Kafka/ConsumerHost/Extensions/ServiceCollectionExtensions.cs

using Kafka.ConsumerHost.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Kafka.ConsumerHost.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddKafkaConsumer<T>(
            this IServiceCollection services,
            Action<KafkaConsumerSettings> configureOptions) where T : class
        {
            services.Configure(configureOptions);
            services.AddHostedService<KafkaConsumerService<T>>();
            return services;
        }
    }
}