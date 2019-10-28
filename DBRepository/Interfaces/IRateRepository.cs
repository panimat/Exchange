using Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace DBRepository.Interfaces
{
    public interface IRateRepository
    {
        Task<IEnumerable<Rate>> GetRates();
        Task<Rate> GetRates(string pair);
        double GetAsk(string pair);
        double GetBid(string pair);
        Task AddRate(Rate rate);
    }

}
