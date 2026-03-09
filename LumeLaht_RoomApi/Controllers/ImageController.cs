using LumeLaht_RoomApi.Application.IServices;
using Microsoft.AspNetCore.Mvc;

namespace LumaCove_RoomApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ImageController : ControllerBase
{
    private readonly IImageService _imageService;
    private readonly ILogger<ImageController> _logger;

    public ImageController(IImageService imageService, ILogger<ImageController> logger)
    {
        _imageService = imageService;
        _logger = logger;
    }

    [HttpPost("upload")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Upload(IFormFile file, CancellationToken cancellationToken)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file provided");

        using var stream = file.OpenReadStream();
        var url = await _imageService.UploadImageAsync(stream, file.FileName, cancellationToken);

        _logger.LogInformation("Image uploaded: {Url}", url);
        return Ok(new { url });
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete([FromQuery] string publicId, CancellationToken cancellationToken)
    {
        await _imageService.DeleteImageAsync(publicId, cancellationToken);
        return NoContent();
    }
}
