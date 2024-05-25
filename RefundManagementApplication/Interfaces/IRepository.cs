namespace RefundManagementApplication.Interfaces
{
    public interface IRepository<K,T> where T : class
    {
        public Task<T> Get(K key);
        public Task<IEnumerable<T>> Get();
        public Task<T> Add(T entity);
        public Task<T> Update(T entity);
        public Task<T> Delete(K key);
    }
}
