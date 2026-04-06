namespace RoomService.Application.IServices;

public interface IImageService
{
    Task<string> UploadImageAsync(Stream imageStream, string fileName, CancellationToken cancellationToken = default);
    Task DeleteImageAsync(string publicId, CancellationToken cancellationToken = default);
}
