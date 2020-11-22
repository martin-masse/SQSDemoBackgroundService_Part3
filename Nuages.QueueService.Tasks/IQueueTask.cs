using System.Threading.Tasks;

namespace Nuages.QueueService.Tasks
{
    public interface IQueueTask
    {
        Task ExecuteAsync(string jsonPayload);
    }
}