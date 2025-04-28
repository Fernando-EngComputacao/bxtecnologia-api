using Amazon.S3.Model;

namespace BXTecnologia.API.Services.Interfaces;

public interface ICustomerImageService
{
    Task<PutObjectResponse> UploadImageAsync(Guid id, IFormFile file);

    Task<PutObjectResponse> UpdateImageAsync(Guid id, string fileName, int width, int height);
    Task<GetObjectResponse?> GetImageAsync(Guid id, string nameImage);

    Task<List<string?>> GetAllImagesByCustomerAsync(Guid id);

    Task<DeleteObjectResponse> DeleteImageAsync(Guid id);
}