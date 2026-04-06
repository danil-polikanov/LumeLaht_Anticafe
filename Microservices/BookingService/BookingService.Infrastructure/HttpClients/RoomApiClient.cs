using System.Net.Http.Json;
using BookingService.Application.Dto;
using BookingService.Application.IServices;
using Microsoft.Extensions.Logging;

namespace BookingService.Infrastructure.HttpClients
{
    public class RoomApiClient : IRoomApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<RoomApiClient> _logger;

        public RoomApiClient(HttpClient httpClient, ILogger<RoomApiClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<RoomDto?> GetRoomByIdAsync(Guid roomId, CancellationToken ct)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/room/{roomId}", ct);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("RoomService returned {StatusCode} for room {RoomId}",
                        response.StatusCode, roomId);
                    return null;
                }

                return await response.Content.ReadFromJsonAsync<RoomDto>(ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch room {RoomId} from RoomService", roomId);
                return null;
            }
        }
    }
}
