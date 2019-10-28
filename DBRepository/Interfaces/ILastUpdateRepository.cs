using Models;
using System.Threading.Tasks;

namespace DBRepository.Interfaces
{
    public interface ILastUpdateRepository
    {
        Task<LastUpdate> GetLastUpdate();
        Task SetLastUpdate();
    }
}
