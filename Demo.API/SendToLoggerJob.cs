using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Nuages.QueueService.Jobs;

namespace Demo.API
{
    public class SendToLoggerJob : Job<SendToLoggerJobData>
    {
        private readonly ILogger<SendToLoggerJob> _logger;

        public SendToLoggerJob(ILogger<SendToLoggerJob> logger )
        {
            _logger = logger;
        }
        
        protected override async Task ExecuteAsync(SendToLoggerJobData data)
        {
            _logger.LogInformation(new string('=', 50));
            _logger.LogInformation($"Message :  {data.Message}");
            _logger.LogInformation(new string('=', 50));
            
            await Task.FromResult(0);
        }
    }
    
    public class SendToLoggerJobData
    {
        public string Message { get; set; }
    }
}