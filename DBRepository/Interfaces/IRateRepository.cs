using Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace DBRepository.Interfaces
{
    public interface IRateRepository : IRepository<Rate>
    {
        double GetAsk(string pair);
        double GetBid(string pair);
        Task<Rate> GetByName(string name);

    }

}
