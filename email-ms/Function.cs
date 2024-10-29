using System.Net;
using Google.Cloud.Functions.Framework;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net.Mail;
using System.IO;

namespace EmailMs;

public class Function : IHttpFunction
{
    private readonly IConfiguration _configuration;

    public Function()
    {
        var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production";

        _configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();
    }

    public async Task HandleAsync(HttpContext context)
    {
        var emailRequest = await JsonSerializer.DeserializeAsync<EmailRequest>(context.Request.Body);

        if (emailRequest == null || string.IsNullOrEmpty(emailRequest.ToEmail))
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync("Invalid request");
            return;
        }

        var smtpHost = _configuration["SMTP_HOST"];
        var smtpPort = int.Parse(_configuration["SMTP_PORT"]);
        var smtpUser = _configuration["SMTP_USER"];
        var smtpPass = _configuration["SMTP_PASS"];
        var fromEmail = _configuration["FROM_EMAIL"];
        var fromName = _configuration["FROM_NAME"];

        var smtpClient = new SmtpClient(smtpHost)
        {
            Credentials = new NetworkCredential(smtpUser, smtpPass),
            UseDefaultCredentials = false,
            EnableSsl = true,
            Port = smtpPort
        };

        var addressFrom = new MailAddress(fromEmail, fromName);
        var addressTo = new MailAddress(emailRequest.ToEmail);

        var mailMessage = new MailMessage(addressFrom, addressTo)
        {
            Subject = emailRequest.Subject,
            Body = emailRequest.HtmlContent,
            IsBodyHtml = true
        };

        if (!string.IsNullOrEmpty(emailRequest.AttachmentBase64) && !string.IsNullOrEmpty(emailRequest.AttachmentFilename))
        {
            byte[] attachmentBytes = Convert.FromBase64String(emailRequest.AttachmentBase64);
            mailMessage.Attachments.Add(new Attachment(new MemoryStream(attachmentBytes), emailRequest.AttachmentFilename));
        }

        try
        {
            await smtpClient.SendMailAsync(mailMessage);

            context.Response.StatusCode = 200;
            await context.Response.WriteAsync("Email sent successfully.");
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync($"Error sending email: {ex.Message}");
        }
    }
}


public class EmailRequest
{
    public string ToEmail { get; set; }
    public string Subject { get; set; }
    public string HtmlContent { get; set; }
    public string AttachmentBase64 { get; set; }
    public string AttachmentFilename { get; set; }
}
