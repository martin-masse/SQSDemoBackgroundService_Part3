namespace Nuages.QueueService
{
    public class QueueMessage
    {
        public string MessageId { get; set; }
        public string Body { get; set; } 
        public string Handle { get; set; } 
    }
}