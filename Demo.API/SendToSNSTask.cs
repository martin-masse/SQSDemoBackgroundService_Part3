using System.Threading.Tasks;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Nuages.QueueService.Tasks;
// ReSharper disable InconsistentNaming

namespace Demo.API
{
    public class SendToSNSTask : QueueTask<SendToSNSTaskData>
    {
        private readonly IAmazonSimpleNotificationService _sns;

        public SendToSNSTask(IAmazonSimpleNotificationService sns )
        {
            _sns = sns;
        }
        
        protected override async Task ExecuteAsync(SendToSNSTaskData data)
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
    
    public class SendToSNSTaskData
    {
        public string Message { get; set; }
        public string Subject { get; set; }
        public string TopicArn { get; set; }
    }
}