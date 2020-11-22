using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Nuages.QueueService;
using Nuages.QueueService.Jobs;
// ReSharper disable InconsistentNaming

namespace Demo.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestsController : ControllerBase
    {
        private readonly IQueueService _queueService;
        private readonly string _queueName;

        public TestsController(IQueueService queueService, IConfiguration configuration)
        {
            _queueService = queueService;
            _queueName = configuration.GetValue<string>("JobWorkerService:QueueName");
        }
        
        [HttpPost("SendToLogger")]
        public async Task<ActionResult<bool>> SendToLogger(string message)
        {
            var queueUrl = await _queueService.GetQueueUrlAsync(_queueName);

            return await _queueService.PublishToQueueAsync(queueUrl, 
                typeof(SendToLoggerJob), 
                new SendToLoggerJobData { Message = message });
        }
        
        [HttpPost("SendToSNS")]
        public async Task<ActionResult<bool>> SendToSNS(SendToSNSJobData message)
        {
            var queueUrl = await _queueService.GetQueueUrlAsync(_queueName);
            
            return await _queueService.PublishToQueueAsync(queueUrl, 
                typeof(SendToSNSJob), 
                message);
        }
        
    }
}