using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace Nuages.QueueService.SQS
{
    public class SqsService : IQueueService
    {
        private readonly IAmazonSQS _amazonSqs;

        public SqsService(IAmazonSQS amazonSqs)
        {
            _amazonSqs = amazonSqs;
        }

        public async Task<string> GetQueueUrlAsync(string queueName)
        {
            try
            {
                var response = await _amazonSqs.GetQueueUrlAsync(new GetQueueUrlRequest
                {
                    QueueName = queueName
                });

                return response.QueueUrl;
            }
            catch (QueueDoesNotExistException)
            {
                //You might want to add additionale exception handling here because that may fail
                var response = await _amazonSqs.CreateQueueAsync(new CreateQueueRequest
                {
                    QueueName = queueName
                });

                return response.QueueUrl;
            }
        }
        
        public async Task<bool> PublishToQueueAsync(string queueUrl, string message)
        {
            await _amazonSqs.SendMessageAsync(new SendMessageRequest
            {
                MessageBody = message,
                QueueUrl = queueUrl

            });

            return true;
        }

        public async Task<List<QueueMessage>> ReceiveMessageAsync(string queueUrl, int maxMessages = 1)
        {
            var request = new ReceiveMessageRequest
            {
                QueueUrl = queueUrl,
                MaxNumberOfMessages = maxMessages
            };

            var messages = await _amazonSqs.ReceiveMessageAsync(request);
            
            return messages.Messages.Select(m => new QueueMessage
            {
                MessageId = m.MessageId,
                Body = m.Body,
                Handle = m.ReceiptHandle
            }).ToList();
        }

        public async Task DeleteMessageAsync(string queueUrl, string id)
        {
            await _amazonSqs.DeleteMessageAsync(new DeleteMessageRequest
            {
                QueueUrl = queueUrl,
                ReceiptHandle = id
            });
        }
    }
}