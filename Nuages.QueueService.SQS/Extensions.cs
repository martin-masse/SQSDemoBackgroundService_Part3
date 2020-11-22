using Amazon.SQS;
using Microsoft.Extensions.DependencyInjection;

namespace Nuages.QueueService.SQS
{
    public static class Extensions
    {
        // ReSharper disable once UnusedMethodReturnValue.Global
        public static IServiceCollection AddSqsService(this IServiceCollection services)
        {
            services.AddAWSService<IAmazonSQS>();
            services.AddScoped<IQueueService, SqsService>();
            
            return services;
        }
    }
}