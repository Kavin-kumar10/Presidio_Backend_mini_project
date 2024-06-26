using Microsoft.EntityFrameworkCore;
using RefundManagementApplication.Context;
using RefundManagementApplication.Interfaces;
using RefundManagementApplication.Models;

namespace RefundManagementApplication.Repositories
{
    public class UserRepository : BaseRepository<User>
    {
        RefundManagementContext _context;

        public UserRepository(RefundManagementContext context) : base(context)
        {
            _context = context;
        }
        public override async Task<IEnumerable<User>> Get()
        {
            var result = await _context.Users.Include(u=>u.Member).ToListAsync();
            return result;
        }
        public override async Task<User> Get(int MemberId)
        {
            var result = _context.Users.Include(u => u.Member).FirstOrDefault((elem) => elem.MemberId == MemberId);
            return result;
        }
    }
}
