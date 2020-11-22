using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable VirtualMemberNeverOverridden.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
// ReSharper disable InvertIf

namespace Nuages.QueueService
{
    public abstract class QueueWorkerService : BackgroundService
    {
        protected string QueueName { get; set; }
        protected int MaxMessages { get; set; } = 10;
        protected int WaitDelayWhenNoMessages { get; set; } = 1;
        
        protected readonly IServiceProvider ServiceProvider;
        protected readonly ILogger<QueueWorkerService> Logger;

        protected QueueWorkerService(IServiceProvider serviceProvider, ILogger<QueueWorkerService> logger)
        {
            ServiceProvider = serviceProvider;
            Logger = logger;
        }
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //The queue service is registered as scope because we might want to have more than one backgroud queue service in the same app.
            //Since the BackgroundWorkerService is not a scoped service, it will throw an exception if we try to inject a scoped service
            //We muste then create a new scope and use is to instantiuate the required service
            //You may register IQueueService as a singleton and inject it in the constructor if you like. Your choice.
            
            using var scope = ServiceProvider.CreateScope();

            var queueService = scope.ServiceProvider.GetRequiredService<IQueueService>();
            
            var queueUrl = await queueService.GetQueueUrlAsync(QueueName);
            
            LogInformation($"Starting polling queue : {QueueName}");
            
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var messages = await queueService.ReceiveMessageAsync(queueUrl, MaxMessages);

                    if (messages.Any())
                    {
                        LogInformation($"{messages.Count} messages received");

                        foreach (var msg in messages)
                        {
                            var result = await ProcessMessageAsync(msg);

                            if (result)
                            {
                                LogInformation($"{msg.MessageId} processed with success");
                                await queueService.DeleteMessageAsync( queueUrl, msg.Handle);
                            }
                        }
                    }
                    else
                    {
                        LogInformation("No message available");
                        await Task.Delay(TimeSpan.FromSeconds(WaitDelayWhenNoMessages), stoppingToken);
                    }
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);
                }
                
            }
        }

        protected abstract Task<bool> ProcessMessageAsync(QueueMessage msg);

        protected virtual void LogInformation(string message)
        {
            Logger.LogInformation(message);
        }
        
        protected virtual void LogError(string message)
        {
            Logger.LogError(message);
        }
    }
}