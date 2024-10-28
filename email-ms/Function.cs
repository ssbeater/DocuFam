using Google.Cloud.Functions.Framework;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace EmailMS
{
    public class MyHttpFunction : IHttpFunction
    {
        private readonly IConfiguration _configuration;

        public MyHttpFunction()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory) // Establece la ruta base como el directorio actual
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
        }

        public async Task HandleAsync(HttpContext context)
        {
            try
            {
                var emailRequest = await JsonSerializer.DeserializeAsync<EmailRequest>(context.Request.Body);

                if (emailRequest == null || string.IsNullOrEmpty(emailRequest.ToEmail))
                {
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsync("Invalid request");
                    return;
                }

                var _sendGridApiKey = _configuration["Settings:SENDGRID_API_KEY"];
                var client = new SendGridClient(_sendGridApiKey);

                var msg = new SendGridMessage
                {
                    From = new EmailAddress("santiagosuarez@fymtech.com", "Santiago Suarez"),
                    Subject = emailRequest.Subject,
                    HtmlContent = emailRequest.HtmlContent
                };

                // Add recipient
                msg.AddTo(new EmailAddress(emailRequest.ToEmail));

                if (!string.IsNullOrEmpty(emailRequest.AttachmentBase64) && !string.IsNullOrEmpty(emailRequest.AttachmentFilename))
                {
                    msg.AddAttachment(
                        emailRequest.AttachmentFilename,
                        emailRequest.AttachmentBase64,
                        "application/octet-stream" // Change MIME type if needed
                    );
                }

                var response = await client.SendEmailAsync(msg);

                // Check if the response is successful
                if (response.StatusCode == System.Net.HttpStatusCode.OK || response.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    context.Response.StatusCode = 200;
                    await context.Response.WriteAsync("Email sent successfully");
                    return;
                }

                context.Response.StatusCode = (int)response.StatusCode;
                var errorContent = await response.Body.ReadAsStringAsync();
                await context.Response.WriteAsync($"Failed to send email: {errorContent}");
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
}
