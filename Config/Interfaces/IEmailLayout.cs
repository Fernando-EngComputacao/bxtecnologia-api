namespace BXTecnologia.API.Config.Interfaces;

public interface IEmailLayout
{
    string GetUserRegistrationEmailTemplate(string userName, string level, string? confirmationLink = null);
    string GetImageProcessingEmailTemplate(string userName, int imageCount, string? viewImagesLink = null);
    string GetImageRegistrationTemplate(string userName, string imageName, string? viewImageLink = null);
}