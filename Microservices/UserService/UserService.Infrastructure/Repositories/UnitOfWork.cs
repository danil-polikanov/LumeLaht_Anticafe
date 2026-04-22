using UserService.Core.Interfaces;
using UserService.Infrastructure.Data;

namespace UserService.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly UserDbContext _context;

        public IUserRepository Users { get; }

        public UnitOfWork(UserDbContext context, IUserRepository users)
        {
            _context = context;
            Users = users;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
            await _context.SaveChangesAsync(cancellationToken);

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
