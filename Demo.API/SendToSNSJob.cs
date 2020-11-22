using System.Threading.Tasks;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Nuages.QueueService.Jobs;
// ReSharper disable InconsistentNaming

namespace Demo.API
{
    public class SendToSNSJob : Job<SendToSNSJobData>
    {
        private readonly IAmazonSimpleNotificationService _sns;

        public SendToSNSJob(IAmazonSimpleNotificationService sns )
        {
            _sns = sns;
        }
        
        protected override async Task ExecuteAsync(SendToSNSJobData data)
        {
            await _sns.PublishAsync(new PublishRequest
            {
                Message = data.Message,
                Subject = data.Subject,
                TopicArn = data.TopicArn
            });

            await Task.FromResult(0);
        }
    }
    
    public class SendToSNSJobData
    {
        public string Message { get; set; }
        public string Subject { get; set; }
        public string TopicArn { get; set; }
    }
}