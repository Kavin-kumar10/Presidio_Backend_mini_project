using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RefundManagementApplication.Context;
using RefundManagementApplication.Exceptions;
using RefundManagementApplication.Interfaces;

namespace RefundManagementApplication.Repositories
{
    public abstract class BaseRepository<T> : IRepository<int, T> where T : class
    {
        RefundManagementContext _context;
        public BaseRepository(RefundManagementContext context)
        {
            _context = context;
        }

        public async Task<T> Add(T entity)
        {
            _context.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<T> Delete(int key)
        {
            var request = await Get(key);
            if (request == null)
                return null;
            _context.Remove(request);
            await _context.SaveChangesAsync();
            return request;
        }

        public virtual async Task<T> Get(int key)
        {
            var result = await _context.Set<T>().FindAsync(key);
            return result;
        }

        public virtual async Task<IEnumerable<T>> Get()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> Update(T entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
            return entity;        
        }
    }
}
