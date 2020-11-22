using System.Text.Json;
using System.Threading.Tasks;

namespace Nuages.QueueService.Tasks
{
    public abstract class QueueTask<T> : IQueueTask
    {
        protected abstract Task ExecuteAsync(T data);

        public virtual async Task ExecuteAsync(string jsonPayload)
        {
            if (!string.IsNullOrEmpty(jsonPayload))
            {
                var data = JsonSerializer.Deserialize<T>(jsonPayload);
                await ExecuteAsync(data);
            }
            else
            {
                await ExecuteAsync((T) default);
            }
            
        }
    }
}