using System.Collections.Generic;
using System.Threading.Tasks;
using Models;

namespace DBRepository.Interfaces
{
    public interface ICurrencyRepository
    {
        Task SetCurrencies(IEnumerable<Currencies> currencies);
        Task<IEnumerable<Currencies>> GetCurrencies();
        IEnumerable<string> GetPairs();
    }
}
