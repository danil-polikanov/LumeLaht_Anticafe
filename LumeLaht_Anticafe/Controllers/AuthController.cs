using LumeLaht_RoomApi.Application.Dto.Auth;
using LumeLaht_RoomApi.Application.IServices;
using Microsoft.AspNetCore.Mvc;

namespace LumeLaht_Anticafe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponse>> Register(
            RegisterRequest request, CancellationToken cancellationToken)
        {
            var result = await _authService.RegisterAsync(request, cancellationToken);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login(
            LoginRequest request, CancellationToken cancellationToken)
        {
            var result = await _authService.LoginAsync(request, cancellationToken);
            return Ok(result);
        }
    }
}
