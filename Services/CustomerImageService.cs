using System.Net;
using Amazon.S3;
using Amazon.S3.Model;
using BXTecnologia.API.Config.Interfaces;
using BXTecnologia.API.Services.Interfaces;
using BXTecnologia.API.Services.Validators;
using FluentValidation;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Processors.Transforms;

namespace BXTecnologia.API.Services;

public class CustomerImageService : ICustomerImageService
{
    private readonly IAmazonS3 _s3;
    private readonly IEmailService _emailService;
    private readonly IEmailLayout _emailLayout;
    private readonly ICustomerService _customer;
    private readonly CustomerImageValidator _imageValidator;
    private readonly CustomerImageUpdateValidator _imageUpdateValidator;
    private readonly CustomerImageGetValidator _imageGetValidator;
    private readonly CustomerImageDeleteValidator _imageDeleteValidator;
    private const string BucketName = "bxtecnologiabucket";

    public CustomerImageService(IAmazonS3 s3, 
        ICustomerService customer, 
        IEmailService emailService, 
        IEmailLayout emailLayout)
    {
        _s3 = s3;
        _customer = customer;
        _emailService = emailService;
        _emailLayout = emailLayout;
        _imageValidator = new CustomerImageValidator();
        _imageUpdateValidator = new CustomerImageUpdateValidator();
        _imageGetValidator = new CustomerImageGetValidator();
        _imageDeleteValidator = new CustomerImageDeleteValidator();
    }

    public async Task<PutObjectResponse> UploadImageAsync(Guid id, IFormFile file)
    {
        var validationResult = await _imageValidator.ValidateAsync(file);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }
        
        var customer = _customer.GetAsync(id).Result;
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
        

        var response =  await _s3.PutObjectAsync(putObjectRequest);
        if (response.HttpStatusCode == HttpStatusCode.OK)
        {
            _emailService.SendEmailAsync(
                customer.Email, 
                "Cadastro de Imagem Concluído", 
                _emailLayout.GetImageRegistrationTemplate(
                    customer.FullName,
                    file.FileName,
                    $"https://localhost:7194/customers/{id}/{timestamp}{extension}/image"));
        }
        
        Console.WriteLine($"response {response}");
        
        return response;
    }

    public async Task<PutObjectResponse> UpdateImageAsync(Guid id, string fileName, int width, int height)
    {
        var validationResult = await _imageUpdateValidator.ValidateAsync((id, fileName, width, height));
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var customer = _customer.GetAsync(id).Result;
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

        var response = await _s3.PutObjectAsync(putObjectRequest);
    
        if (response.HttpStatusCode == HttpStatusCode.OK)
        {
            _emailService.SendEmailAsync(
                customer.Email, 
                "Processamento de Imagem Concluído", 
                _emailLayout.GetImageProcessingEmailTemplate(
                    customer.FullName,
                    1,
                    $"https://localhost:7194/customers/{id}/{fileName}/image"));
        }
    
        return response;
    }
    
    public async Task<GetObjectResponse?> GetImageAsync(Guid id, string nameImage)
    {
        var validationResult = await _imageGetValidator.ValidateAsync((id, nameImage));
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

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
    
    public async Task<List<string?>> GetAllImagesByCustomerAsync(Guid id)
    {
        var listRequest = new ListObjectsV2Request
        {
            BucketName = BucketName,
            Prefix = $"profile_images/{id}/" // Lista tudo dentro da "pasta" do id
        };

        var listResponse = await _s3.ListObjectsV2Async(listRequest);

        // Pega apenas os nomes dos arquivos (Keys) da resposta
        var imageNames = listResponse.S3Objects
            .Select(obj => obj.Key.Split("/")[2])
            .ToList();

        return imageNames;
    }




    public async Task<DeleteObjectResponse> DeleteImageAsync(Guid id)
    {
        var validationResult = await _imageDeleteValidator.ValidateAsync(id);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var deleteObjectRequest = new DeleteObjectRequest
        {
            BucketName = BucketName,
            Key = $"images/{id}"
        };

        return await _s3.DeleteObjectAsync(deleteObjectRequest);
    }
}