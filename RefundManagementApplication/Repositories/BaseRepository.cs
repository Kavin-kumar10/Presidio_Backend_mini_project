using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RefundManagementApplication.Context;
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
            throw new NotImplementedException();
        }

        public async Task<T> Delete(int key)
        {
            var request =await Get(key);
            if(request != null)
            {
                _context.Remove(request);
                await _context.SaveChangesAsync();
                return request;
            }

            throw new NotImplementedException();
        }

        public async Task<T> Get(int key)
        {
            return await _context.Set<T>().FindAsync(key);
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<T>> Get()
        {
            return await _context.Set<T>().ToListAsync();
            throw new NotImplementedException();
        }

        public async Task<T> Update(T entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
            throw new NotImplementedException();
        }
    }
}
