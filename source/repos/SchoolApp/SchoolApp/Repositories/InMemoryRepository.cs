using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolApp.Repositories
{
    public class InMemoryRepository<T> : IRepository<T>
    where T : class
    {

        private readonly Dictionary<Guid, T> _storage = [];
        private readonly Func<T, Guid> _getId;
        public void Add(T entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetAll()
        {
            throw new NotImplementedException();
        }

        public T GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Remove(T entity)
        {
            throw new NotImplementedException();
        }

        public void Update(Guid id, T updatedEnttity)
        {
            throw new NotImplementedException();
        }
    }
}
