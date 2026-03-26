using LumeLaht_RoomApi.Application.Dto.Auth;

namespace LumeLaht_RoomApi.Application.IServices
{
    public interface IAuthService
    {
        Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken);
        Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken);
    }
}
