using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Nuages.QueueService.Tasks
{
    public static class Extensions
    {
        public static async Task<bool> PublishToQueueAsync<T>(this IQueueService queueService, string queueUrl, Type type, T taskData)
        {
            var taskDef = new TaskDefinition
            {
                AssemblyQualifiedName = type.AssemblyQualifiedName,
                JsonPayload = JsonSerializer.Serialize(taskData)
            };
            
            return  await queueService.PublishToQueueAsync(queueUrl, JsonSerializer.Serialize(taskDef));
        }
    }
}