using System.Net;
using Amazon.S3;
using Amazon.S3.Model;
using BXTecnologia.API.Client.AWS;
using BXTecnologia.API.Config.Interfaces;
using BXTecnologia.API.Services.Interfaces;
using BXTecnologia.API.Services.Validators;
using BXTecnologia.API.Validation;
using FluentValidation;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Processors.Transforms;

namespace BXTecnologia.API.Services;

public class CustomerImageService : ICustomerImageService
{
    private readonly IAmazonS3 _s3;
    private readonly AwsConfig _awsConfig;
    private readonly IEmailLayout _emailLayout;
    private readonly ICustomerService _customer;
    private readonly IEmailService _emailService;
    private readonly CustomerImageValidator _imageValidator;
    private readonly CustomerImageGetValidator _imageGetValidator;
    private readonly CustomerImageUpdateValidator _imageUpdateValidator;
    private readonly CustomerImageDeleteValidator _imageDeleteValidator;

    public CustomerImageService(IAmazonS3 s3, 
        ICustomerService customer, 
        IEmailService emailService, 
        IEmailLayout emailLayout,
        IOptions<AwsConfig> awsConfig)
    {
        _s3 = s3;
        _customer = customer;
        _emailLayout = emailLayout;
        _awsConfig = awsConfig.Value;
        _emailService = emailService;
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

        // Is Beginner with 5 files or Premium
        await CanContinueAsync(id);
        
        var customer = await _customer.GetAsync(id);
        var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        var extension = Path.GetExtension(file.FileName);
        
        var putObjectRequest = new PutObjectRequest
        {
            BucketName = _awsConfig.BucketNameS3,
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
            BucketName = _awsConfig.BucketNameS3,
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
                BucketName = _awsConfig.BucketNameS3,
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
        try
        {
            var listRequest = new ListObjectsV2Request
            {
                BucketName = _awsConfig.BucketNameS3,
                Prefix = $"profile_images/{id}/" // Lista tudo dentro da "pasta" do id
            };

            var listResponse = await _s3.ListObjectsV2Async(listRequest);

            // Verificar se há objetos antes de processar
            if (listResponse.S3Objects == null || !listResponse.S3Objects.Any())
            {
                return new List<string?>(); // Retorna lista vazia se não houver objetos
            }

            // Pega apenas os nomes dos arquivos (Keys) da resposta
            var imageNames = listResponse.S3Objects
                .Where(obj => obj.Key != null && obj.Key.Split("/").Length > 2) // Verifica se a Key tem o formato esperado
                .Select(obj => obj.Key.Split("/")[2])
                .ToList();
            
            return imageNames;
        }
        catch (AmazonS3Exception ex)
        {
            // Log the exception
            Console.WriteLine($"S3 Error: {ex.Message}");
            return new List<string?>(); // Return empty list on error
        }
        catch (Exception ex)
        {
            // Log the exception
            Console.WriteLine($"Error in GetAllImagesByCustomerAsync: {ex.Message}");
            return new List<string?>(); // Return empty list on error
        }
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
            BucketName = _awsConfig.BucketNameS3,
            Key = $"images/{id}"
        };

        return await _s3.DeleteObjectAsync(deleteObjectRequest);
    }

    private async Task CanContinueAsync(Guid id)
    {
        try
        {
            var customer = await _customer.GetAsync(id);
            
            var date = DateTime.UtcNow.ToString("yyyyMMdd");
            var imagesList = await GetAllImagesByCustomerAsync(id);
            
            if (imagesList == null) imagesList = new List<string?>();
            
            var imagesByToday = imagesList
                .Where(item => item != null) 
                .Count(img => img.StartsWith(date));

            if (customer.Level.StartsWith("Beginner"))
            {
                ValidationExceptionHelper.ThrowIf(
                    imagesByToday >= 5, 
                    "Limite de 5 imagens excedido! Mude para o plano Prêmium para uploads ilimitados!"
                );
            }
        }
        catch (Exception ex) when (!(ex is ApiException))
        {
            // Wrap any non-ApiException in an ApiException
            throw new ApiException($"Erro ao verificar limites de imagem: {ex.Message}", HttpStatusCode.InternalServerError);
        }
    }
}