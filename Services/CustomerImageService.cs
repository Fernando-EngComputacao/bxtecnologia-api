using Amazon.S3;
using Amazon.S3.Model;
using BXTecnologia.API.Client;
using BXTecnologia.API.Services.Interfaces;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Processors.Transforms;

namespace BXTecnologia.API.Services;

public class CustomerImageService : ICustomerImageService
{
    private readonly IAmazonS3 _s3;
    private readonly Email _emailConfig;
    private const string BucketName = "bxtecnologiabucket";

    public CustomerImageService(IAmazonS3 s3, IOptions<Email> emailOptions)
    {
        _s3 = s3;
        _emailConfig = emailOptions.Value;
    }

    public async Task<PutObjectResponse> UploadImageAsync(Guid id, IFormFile file)
    {
        var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss"); // Gera tipo 20250426155300
        var extension = Path.GetExtension(file.FileName);
        
        var putObjectRequest = new PutObjectRequest
        {
            BucketName = BucketName,
            Key = $"profile_images/{id}/{timestamp}{extension}",
            ContentType = file.ContentType,
            InputStream = file.OpenReadStream(),
            Metadata =
            {
                ["x-amz-meta-originalname"] = file.FileName,
                ["x-amz-meta-extension"] = Path.GetExtension(file.FileName),
            }
        };

        return await _s3.PutObjectAsync(putObjectRequest);
    }

    public async Task<PutObjectResponse> UpdateImageAsync(Guid id, string fileName, int width, int height)
    {
        var file = await GetImageAsync(id, fileName);
        if (file == null)
        {
            throw new Exception("Imagem não encontrada.");
        }

        var extension = Path.GetExtension(fileName);
        var key = $"profile_images/{id}/{fileName.Split('.')[0]}{extension}";

        await using var inputStream = file.ResponseStream; 
        await using var outStream = new MemoryStream();

        // Carrega e processa a imagem
        using (var image = await Image.LoadAsync(inputStream))
        {
            image.Mutate(x => 
                x.Resize(width, height, LanczosResampler.Lanczos3)
                    .Grayscale()
                );
            await image.SaveAsync(outStream, image.DetectEncoder(fileName));
        }

        outStream.Position = 0;

        var putObjectRequest = new PutObjectRequest
        {
            BucketName = BucketName,
            Key = key,
            ContentType = file.Headers.ContentType,
            InputStream = outStream,
            Metadata =
            {
                ["x-amz-meta-originalname"] = fileName,
                ["x-amz-meta-extension"] = extension,
                ["x-amz-meta-resized"] = true.ToString()
            }
        };

        return await _s3.PutObjectAsync(putObjectRequest);
    }


    public async Task<GetObjectResponse?> GetImageAsync(Guid id, string nameImage)
    {
        Console.WriteLine("EMAIL", _emailConfig, "");

        try
        {
            var getObjectRequest = new GetObjectRequest
            {
                BucketName = BucketName,
                Key = $"profile_images/{id}/{nameImage}" // <-- agora usa o id e o nome da imagem
            };
        
            var response = await _s3.GetObjectAsync(getObjectRequest);
            
            return response;
        }
        catch (AmazonS3Exception ex) when (ex.Message.Contains("The specified key does not exist"))
        {
            return null;
        }
    }
    
    public async Task<List<GetObjectResponse?>> GetAllImagesByCustomerAsync(Guid id)
    {
        var listRequest = new ListObjectsV2Request
        {
            BucketName = BucketName,
            Prefix = $"profile_images/{id}/" // Lista tudo dentro da "pasta" do id
        };

        var listResponse = await _s3.ListObjectsV2Async(listRequest);

        var responses = new List<GetObjectResponse>();

        foreach (var s3Object in listResponse.S3Objects)
        {
            // Ignora "pastas" vazias que o S3 pode listar (caso queira ser mais seguro)
            if (string.IsNullOrEmpty(s3Object.Key) || s3Object.Size == 0)
                continue;

            var getRequest = new GetObjectRequest
            {
                BucketName = BucketName,
                Key = s3Object.Key
            };

            var getResponse = await _s3.GetObjectAsync(getRequest);
            responses.Add(getResponse);
        }

        return responses ?? null;
    }



    public async Task<DeleteObjectResponse> DeleteImageAsync(Guid id)
    {
        var deleteObjectRequest = new DeleteObjectRequest
        {
            BucketName = BucketName,
            Key = $"images/{id}"
        };

        return await _s3.DeleteObjectAsync(deleteObjectRequest);
    }
}