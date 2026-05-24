using LumeLaht_RoomApi.Application.Dto;
using LumeLaht_RoomApi.Application.IServices;
using LumeLaht_RoomApi.Core_.Entities;
using LumeLaht_RoomApi.Core_.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LumeLaht_Anticafe.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ImageController : ControllerBase
{
    private readonly IImageService _imageService;
    private readonly IUnitOfWork _uow;
    private readonly ILogger<ImageController> _logger;

    public ImageController(IImageService imageService, IUnitOfWork uow, ILogger<ImageController> logger)
    {
        _imageService = imageService;
        _uow = uow;
        _logger = logger;
    }

    [HttpPost("upload/{roomId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Upload(Guid roomId, IFormFile file, [FromQuery] bool isMain = false, CancellationToken cancellationToken = default)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file provided");

        var room = await _uow.Rooms.GetByIdAsync(roomId, cancellationToken);
        if (room == null)
            return NotFound($"Room with ID {roomId} not found");

        using var stream = file.OpenReadStream();
        var url = await _imageService.UploadImageAsync(stream, file.FileName, cancellationToken);

        var publicId = ExtractPublicId(url);

        var roomImage = new RoomImage
        {
            ImageId = Guid.NewGuid(),
            Url = url,
            CloudinaryPublicId = publicId,
            IsMain = isMain,
            RoomId = roomId
        };

        await _uow.RoomImages.AddAsync(roomImage, cancellationToken);

        _logger.LogInformation("Image uploaded for room {RoomId}: {Url}", roomId, url);
        return Ok(new RoomImagesResponseDto
        {
            ImageId = roomImage.ImageId,
            Url = roomImage.Url,
            CloudinaryPublicId = roomImage.CloudinaryPublicId,
            IsMain = roomImage.IsMain,
            RoomId = roomImage.RoomId
        });
    }

    [HttpDelete("{imageId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid imageId, CancellationToken cancellationToken)
    {
        var image = await _uow.RoomImages.GetByIdAsync(imageId, cancellationToken);
        if (image == null)
            return NotFound($"Image with ID {imageId} not found");

        if (!string.IsNullOrEmpty(image.CloudinaryPublicId))
            await _imageService.DeleteImageAsync(image.CloudinaryPublicId, cancellationToken);

        await _uow.RoomImages.DeleteAsync(imageId, cancellationToken);
        return NoContent();
    }

    private static string? ExtractPublicId(string url)
    {
        var uri = new Uri(url);
        var path = uri.AbsolutePath;
        var uploadIndex = path.IndexOf("/upload/", StringComparison.Ordinal);
        if (uploadIndex < 0) return null;

        var afterUpload = path[(uploadIndex + "/upload/".Length)..];
        if (afterUpload.StartsWith('v') && afterUpload.Contains('/'))
            afterUpload = afterUpload[(afterUpload.IndexOf('/') + 1)..];

        var dotIndex = afterUpload.LastIndexOf('.');
        return dotIndex > 0 ? afterUpload[..dotIndex] : afterUpload;
    }
}
