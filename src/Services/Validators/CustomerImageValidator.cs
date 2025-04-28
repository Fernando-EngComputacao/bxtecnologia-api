using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace BXTecnologia.API.Services.Validators;

public class CustomerImageValidator : AbstractValidator<IFormFile>
{
    private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
    private const int MaxFileSizeInMb = 5;
    private const int MaxFileSizeInBytes = MaxFileSizeInMb * 1024 * 1024; // 5MB

    public CustomerImageValidator()
    {
        RuleFor(x => x)
            .NotNull().WithMessage("A imagem é obrigatória");

        RuleFor(x => x.Length)
            .LessThanOrEqualTo(MaxFileSizeInBytes)
            .WithMessage($"O tamanho da imagem não pode exceder {MaxFileSizeInMb}MB");

        RuleFor(x => x.FileName)
            .NotEmpty().WithMessage("O nome do arquivo é obrigatório")
            .Must(fileName => _allowedExtensions.Contains(Path.GetExtension(fileName).ToLowerInvariant()))
            .WithMessage($"A extensão do arquivo deve ser uma das seguintes: {string.Join(", ", _allowedExtensions)}");

        RuleFor(x => x.ContentType)
            .NotEmpty().WithMessage("O tipo de conteúdo é obrigatório")
            .Must(contentType => contentType.StartsWith("image/"))
            .WithMessage("O arquivo deve ser uma imagem");
    }
}

public class CustomerImageUpdateValidator : AbstractValidator<(Guid id, string fileName, int width, int height)>
{
    public CustomerImageUpdateValidator()
    {
        RuleFor(x => x.id)
            .NotEmpty().WithMessage("O ID do cliente é obrigatório");

        RuleFor(x => x.fileName)
            .NotEmpty().WithMessage("O nome do arquivo é obrigatório");

        RuleFor(x => x.width)
            .GreaterThan(0).WithMessage("A largura deve ser maior que zero")
            .LessThanOrEqualTo(2000).WithMessage("A largura não pode exceder 2000 pixels");

        RuleFor(x => x.height)
            .GreaterThan(0).WithMessage("A altura deve ser maior que zero")
            .LessThanOrEqualTo(2000).WithMessage("A altura não pode exceder 2000 pixels");
    }
}

public class CustomerImageGetValidator : AbstractValidator<(Guid id, string nameImage)>
{
    public CustomerImageGetValidator()
    {
        RuleFor(x => x.id)
            .NotEmpty().WithMessage("O ID do cliente é obrigatório");

        RuleFor(x => x.nameImage)
            .NotEmpty().WithMessage("O nome da imagem é obrigatório");
    }
}

public class CustomerImageDeleteValidator : AbstractValidator<Guid>
{
    public CustomerImageDeleteValidator()
    {
        RuleFor(x => x)
            .NotEmpty().WithMessage("O ID do cliente é obrigatório");
    }
}
