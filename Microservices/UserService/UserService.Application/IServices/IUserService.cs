using UserService.Application.Dto;

namespace UserService.Application.IServices
{
    public interface IUserService
    {
        Task<UserResponse?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken);
    }
}
