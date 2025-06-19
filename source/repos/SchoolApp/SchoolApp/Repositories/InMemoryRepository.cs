using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolApp.Repositories
{
    public class InMemoryRepository<T>(Func<T, Guid> getId) : IRepository<T>
    where T : class
    {

        private readonly Dictionary<Guid, T> _storage = [];
        private readonly Func<T, Guid> _getId = getId;

        public Dictionary<Guid, T> Storage => _storage;

        public Func<T, Guid> GetId => _getId;

        public void Add(T entity)
        {
            var id = _getId(entity);
            _storage[id] = entity;
        }

        public IEnumerable<T> GetAll()
        {
            return [.. _storage.Values];
        }

        public T GetById(Guid id)
        {
          _storage.TryGetValue(id, out var entity);
            return entity;
        }

        public void Remove(Guid id)
        {
            _storage.Remove(id);
        }

        public void Update(Guid id, T updatedEnttity)
        {
           if (_storage.ContainsKey(id))
            {
                _storage[id] = updatedEnttity;
            }
            else
            {
                throw new KeyNotFoundException($"Entity with ID {id} not found.");
            }
    }
}
