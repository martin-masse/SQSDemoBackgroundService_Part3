using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Nuages.QueueService.Jobs
{
    public static class Extensions
    {
        public static async Task<bool> PublishToQueueAsync<T>(this IQueueService queueService, string queueUrl, Type type, T jobData)
        {
            var jobDef = new JobDefinition
            {
                AssemblyQualifiedName = type.AssemblyQualifiedName,
                JsonPayload = JsonSerializer.Serialize(jobData)
            };
            
            return  await queueService.PublishToQueueAsync(queueUrl, JsonSerializer.Serialize(jobDef));
        }
    }
}