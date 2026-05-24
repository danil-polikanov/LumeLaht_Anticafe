namespace UserService.Core.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        void Dispose();
    }
}
