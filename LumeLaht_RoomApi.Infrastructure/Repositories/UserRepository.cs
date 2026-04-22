using LumeLaht_RoomApi.Core_.Entities.User;
using LumeLaht_RoomApi.Core_.Interfaces;
using LumeLaht_RoomApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LumeLaht_RoomApi.Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context) { }

        public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
            => await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }
}
