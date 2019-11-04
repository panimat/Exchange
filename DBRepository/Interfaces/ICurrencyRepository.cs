using System.Collections.Generic;
using System.Threading.Tasks;
using Models;

namespace DBRepository.Interfaces
{
    public interface ICurrencyRepository : IRepository<Currencies>
    {
        Task AddAllCurrencies(IEnumerable<Currencies> currencies);
        IEnumerable<string> GetPairs();
    }
}
