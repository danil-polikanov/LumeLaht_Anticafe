using UserService.Core.Entities;

namespace UserService.Core.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<User> Users { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        void Dispose();
    }
}
