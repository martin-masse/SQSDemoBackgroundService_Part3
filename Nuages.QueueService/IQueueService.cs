using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nuages.QueueService
{
    public interface IQueueService
    {
        Task<string> GetQueueUrlAsync(string queueName);
        
        Task<bool> PublishToQueueAsync(string queueUrl, string message);
        
        Task<List<QueueMessage>> ReceiveMessageAsync(string queueUrl, int maxMessages = 1);

        Task DeleteMessageAsync(string queueUrl, string id);
    }
}