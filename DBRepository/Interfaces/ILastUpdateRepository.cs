using Models;
using System.Threading.Tasks;

namespace DBRepository.Interfaces
{
    public interface ILastUpdateRepository : IRepository<LastUpdate>
    {
        Task<LastUpdate> GetLastUpdate();
    }
}
