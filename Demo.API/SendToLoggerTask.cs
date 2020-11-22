using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Nuages.QueueService.Tasks;

namespace Demo.API
{
    public class SendToLoggerTask : QueueTask<SendToLoggerTaskData>
    {
        private readonly ILogger<SendToLoggerTask> _logger;

        public SendToLoggerTask(ILogger<SendToLoggerTask> logger )
        {
            _logger = logger;
        }
        
        protected override async Task ExecuteAsync(SendToLoggerTaskData data)
        {
            _logger.LogInformation(new string('=', 50));
            _logger.LogInformation($"Message :  {data.Message}");
            _logger.LogInformation(new string('=', 50));
            
            await Task.FromResult(0);
        }
    }
    
    public class SendToLoggerTaskData
    {
        public string Message { get; set; }
    }
}