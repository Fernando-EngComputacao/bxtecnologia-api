namespace BXTecnologia.API.Config.Interfaces;

public interface IEmailLayout
{
    string GetConfirmationEmailTemplate(string userName, string? confirmationLink = null);
    string GetImageProcessingEmailTemplate(string userName, int imageCount, string? viewImagesLink = null);
}