using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Nuages.QueueService.Tasks
{
    public class TaskWorkerService : QueueWorkerService
    {

        public TaskWorkerService(IServiceProvider serviceProvider, 
                                        ILogger<QueueWorkerService> logger, 
                                        IConfiguration configuration) : base(serviceProvider, logger)
        {
             QueueName = configuration.GetValue<string>("TaskWorkerService:QueueName");
        }
        
        protected override async Task<bool> ProcessMessageAsync(QueueMessage msg)
        {
            try
            {
                var taskDefinition = JsonSerializer.Deserialize<TaskDefinition>(msg.Body);
                if (taskDefinition == null || string.IsNullOrEmpty(taskDefinition.AssemblyQualifiedName))
                {
                    throw new Exception($"Can't process message : {msg.MessageId}");
                }

                var type = Type.GetType(taskDefinition.AssemblyQualifiedName!);
                if (type == null)
                {
                    throw new Exception(
                        $"Can't process message, type not found : {msg.MessageId} {taskDefinition.AssemblyQualifiedName}");
                }
                
                var task = (IQueueTask) ActivatorUtilities.CreateInstance(ServiceProvider, type);

                await task.ExecuteAsync(taskDefinition.JsonPayload);
                
                return true;
            }
            catch (Exception e)
            {
                Logger.LogError(e.Message);
                return false;
            }
          
        }
    }
}