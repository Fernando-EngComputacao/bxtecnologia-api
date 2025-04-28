using BXTecnologia.API.Services.Interfaces;

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
    private readonly string _clientId;
    private readonly string _clientSecret;
    private readonly string _refreshToken;
    private readonly string _fromEmail;
    private readonly string _fromName;

    public EmailService(string clientId, string clientSecret, string refreshToken, string fromEmail, string fromName)
    {
        _clientId = clientId;
        _clientSecret = clientSecret;
        _refreshToken = refreshToken;
        _fromEmail = fromEmail;
        _fromName = fromName;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_fromName, _fromEmail));
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

            var accessToken = await GetAccessTokenAsync();

            var oauth2 = new SaslMechanismOAuth2(_fromEmail, accessToken);

            await client.AuthenticateAsync(oauth2);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao enviar o e-mail: {ex.Message}");
            throw;
        }
    }

    public async Task<string> GetAccessTokenAsync()
    {
        using var httpClient = new HttpClient();

        var tokenEndpoint = "https://oauth2.googleapis.com/token";
        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("grant_type", "refresh_token"),
            new KeyValuePair<string, string>("client_id", _clientId),
            new KeyValuePair<string, string>("client_secret", _clientSecret),
            new KeyValuePair<string, string>("refresh_token", _refreshToken)
        });

        var response = await httpClient.PostAsync(tokenEndpoint, content);
        response.EnsureSuccessStatusCode(); // lança erro automático se falhar

        var responseJson = await response.Content.ReadAsStringAsync();
        using var jsonDocument = JsonDocument.Parse(responseJson);

        if (jsonDocument.RootElement.TryGetProperty("access_token", out var accessTokenElement))
        {
            return accessTokenElement.GetString();
        }

        throw new Exception("Falha ao obter o Access Token.");
    }
}
