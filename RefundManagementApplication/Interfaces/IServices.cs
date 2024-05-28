using RefundManagementApplication.Models;

namespace RefundManagementApplication.Interfaces
{
    public interface IServices<K,T> where T : class
    {
        public Task<IList<T>> CreateMultiple(IList<T> Entities);
        public Task<IEnumerable<T>> GetAll();
        public Task<T> GetById(K Key);
        public Task<T> Create(T Entity);
        public Task<T> Update(T Entity);
        public Task<T> Delete(K key);

    }
}
