using BXTecnologia.API.Config;
using BXTecnologia.API.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace BXTecnologia.API.Services;

using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

public class EmailService : IEmailService
{
    private readonly EmailSettings _emailConfig;

    public EmailService(IOptions<EmailSettings> settings)
    {
        _emailConfig = settings.Value;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        Console.WriteLine($"{_emailConfig.FromEmail} {_emailConfig.FromName} {_emailConfig.AppPassword}");
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_emailConfig.FromName, _emailConfig.FromEmail));
        message.To.Add(MailboxAddress.Parse(toEmail));
        message.Subject = subject;

        message.Body = new TextPart("html") // Ou "plain" para texto puro
        {
            Text = body
        };

        using var client = new SmtpClient();
        try
        {
            await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);

            // Use app password authentication instead of OAuth2
            await client.AuthenticateAsync(_emailConfig.FromEmail, _emailConfig.AppPassword);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao enviar o e-mail: {ex.Message}");
            throw;
        }
    }


}
