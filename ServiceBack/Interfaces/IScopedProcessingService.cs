using System.Threading;
using System.Threading.Tasks;

namespace ServiceBack.Interfaces
{
    internal interface IScopedProcessingService
    {
        Task WriteDataRate(CancellationToken stoppingToken);
    }
}
