using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolApp.Repositories
{
    public interface IRepository<T>
    {
        void Add(T entity);
        void Remove(Guid id);
        T GetById(Guid id);
        IEnumerable<T> GetAll();
        void Update(Guid id, T updatedEnttity);

    }
}
