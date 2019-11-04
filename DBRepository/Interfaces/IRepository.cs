using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DBRepository.Interfaces
{
    public interface IRepository<T> : IDisposable
                where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task Add(T item); // create object
        void Update(T item); // update object
        //void Delete(int id); // delete by id
        void Save();  // save changes
    }
}
