using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using LumeLaht_RoomApi.Application.IServices;
using LumeLaht_RoomApi.Application.Settings;
using Microsoft.Extensions.Options;

namespace LumeLaht_RoomApi.Application.Services;

public class ImageService : IImageService
{
    private readonly Cloudinary _cloudinary;
    private readonly string _folder;

    public ImageService(IOptions<CloudinarySettings> settings)
    {
        var s = settings.Value;
        var account = new Account(s.CloudName, s.ApiKey, s.ApiSecret);
        _cloudinary = new Cloudinary(account);
        _folder = s.Folder;
    }

    public async Task<string> UploadImageAsync(Stream imageStream, string fileName, CancellationToken cancellationToken = default)
    {
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(fileName, imageStream),
            Folder = _folder,
            UseFilename = false,
            UniqueFilename = true,
            Overwrite = false
        };

        var result = await _cloudinary.UploadAsync(uploadParams);

        if (result.Error != null)
            throw new InvalidOperationException(result.Error.Message);

        return result.SecureUrl.ToString();
    }

    public async Task DeleteImageAsync(string publicId, CancellationToken cancellationToken = default)
    {
        var deleteParams = new DeletionParams(publicId);
        await _cloudinary.DestroyAsync(deleteParams);
    }
}
