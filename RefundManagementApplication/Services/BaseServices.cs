using RefundManagementApplication.Exceptions;
using RefundManagementApplication.Interfaces;
using RefundManagementApplication.Models;

namespace RefundManagementApplication.Services
{
    public abstract class BaseServices<T> : IServices<int, T> where T : class
    {
        private readonly IRepository<int, T> _repo;

        public BaseServices(IRepository<int, T> repo)
        {
            _repo = repo;
        }

        public async Task<IList<T>> CreateMultiple(IList<T> Entities)
        {
            IList<T> list = new List<T>();  
            foreach(var entity in Entities)
            {
                var result = await _repo.Add(entity);
                list.Add(result);
            }
            if(list != null)
            {
                return Entities;
            }
            throw new UnableToCreateException();
        }
        public async virtual Task<T> Create(T Entity)
        {
            var result = await _repo.Add(Entity);
            if (result != null)
            {
                return result;
            }
            throw new UnableToCreateException();
        }

        public Task<T> Delete(int Key)
        {
            var result = _repo.Delete(Key);
            if (result != null)
                return result;
            throw new NotFoundException();
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            var result = await _repo.Get();
            if (result != null)
            {
                return result;
            }
            throw new NotFoundException();
        }

        public async Task<T> GetById(int Key)
        {
            var result = await _repo.Get(Key);
            if (result == null)
            {
                throw new NotFoundException();
            }
            return result;
        }

        public Task<T> Update(T Entity)
        {
            var result = _repo.Update(Entity);
            if (result != null)
            {
                return result;
            }
            throw new NotFoundException();
        }
    }
}
