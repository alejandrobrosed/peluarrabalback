using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace back.servicios
{
    public class CloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(IConfiguration configuration)
        {
            var cloudinaryUrl = configuration["CLOUDINARY_URL"]?.Trim();

            var cloudName = configuration["Cloudinary:CloudName"]?.Trim();
            var apiKey = configuration["Cloudinary:ApiKey"]?.Trim();
            var apiSecret = configuration["Cloudinary:ApiSecret"]?.Trim();

            Account account;
            if (!string.IsNullOrWhiteSpace(cloudinaryUrl))
            {
                account = new Account(cloudinaryUrl);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(cloudName) || string.IsNullOrWhiteSpace(apiKey) || string.IsNullOrWhiteSpace(apiSecret))
                {
                    throw new InvalidOperationException("Cloudinary no está configurado correctamente (cloud_name/api_key/api_secret).");
                }

                account = new Account(cloudName, apiKey, apiSecret);
            }

            _cloudinary = new Cloudinary(account);
        }

        public async Task<string> UploadAvatarAsync(IFormFile file, int userId)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("Archivo no válido");
            }

            byte[] fileBytes;
            await using (var sourceStream = file.OpenReadStream())
            using (var memoryStream = new MemoryStream())
            {
                await sourceStream.CopyToAsync(memoryStream);
                fileBytes = memoryStream.ToArray();
            }
            await using var uploadStream = new MemoryStream(fileBytes);

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, uploadStream),
                Folder = "peluarrabal/avatars",
                PublicId = $"user_{userId}",
                Overwrite = true
            };

            var result = await _cloudinary.UploadAsync(uploadParams);
            if (result.Error != null)
            {
                throw new Exception(result.Error.Message);
            }

            return result.SecureUrl.ToString();
        }
    }
}
