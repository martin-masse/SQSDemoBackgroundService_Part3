using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Nuages.QueueService.Jobs
{
    public class JobWorkerService : QueueWorkerService
    {

        public JobWorkerService(IServiceProvider serviceProvider, 
                                        ILogger<QueueWorkerService> logger, 
                                        IConfiguration configuration) : base(serviceProvider, logger)
        {
             QueueName = configuration.GetValue<string>("JobWorkerService:QueueName");
        }
        
        protected override async Task<bool> ProcessMessageAsync(QueueMessage msg)
        {
            try
            {
                var jobDefinition = JsonSerializer.Deserialize<JobDefinition>(msg.Body);
                if (jobDefinition == null || string.IsNullOrEmpty(jobDefinition.AssemblyQualifiedName))
                {
                    throw new Exception($"Can't process message : {msg.MessageId}");
                }

                var type = Type.GetType(jobDefinition.AssemblyQualifiedName!);
                if (type == null)
                {
                    throw new Exception(
                        $"Can't process message, type not found : {msg.MessageId} {jobDefinition.AssemblyQualifiedName}");
                }
                
                var job = (IJob) ActivatorUtilities.CreateInstance(ServiceProvider, type);

                await job.ExecuteAsync(jobDefinition.JsonPayload);
                
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