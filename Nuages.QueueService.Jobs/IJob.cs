using System.Threading.Tasks;

namespace Nuages.QueueService.Jobs
{
    public interface IJob
    {
        Task ExecuteAsync(string jsonPayload);
    }
}