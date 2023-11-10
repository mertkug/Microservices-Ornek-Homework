using Inveon.Services.Email.Messages;
using MailKit.Net.Smtp;
using HandlebarsDotNet;
using Inveon.Services.Email.Models;
using Inveon.Services.Email.Models.Dto;
using RazorLight;

using MimeKit;

namespace Inveon.Services.Email;

public static class MailClient
{
    public static async void SendOrderConfirmationEmail(CheckoutHeaderDto checkoutHeaderDto)
    {
        var message = new MimeMessage();
        var content = await RenderOrderConfirmationTemplate(checkoutHeaderDto);
        message.From.Add(new MailboxAddress("Test", "eecommerce72@gmail.com"));
        message.To.Add(new MailboxAddress($"{checkoutHeaderDto.FirstName} {checkoutHeaderDto.LastName}", checkoutHeaderDto.Email));
        message.Subject = "Order Confirmation";

        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = content
        };

        message.Body = bodyBuilder.ToMessageBody();
        var client = new SmtpClient();
        try
        {
            await client.ConnectAsync("smtp.gmail.com", 465, true);
            await client.AuthenticateAsync("eecommerce72@gmail.com", "xyjnkifgfefisojl");
            await client.SendAsync(message);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        finally
        {
            await client.DisconnectAsync(true);
            client.Dispose();
        }

    }

    private static async Task<string> RenderOrderConfirmationTemplate(CheckoutHeaderDto checkoutHeaderDto)
    {
        var engine = new RazorLightEngineBuilder()
            .UseFileSystemProject(AppDomain.CurrentDomain.BaseDirectory) // Assuming the CSHTML file is in the project directory
            .UseMemoryCachingProvider()
            .Build();

        const string templatePath = "OrderConfirmation.cshtml";
        var template = await File.ReadAllTextAsync(templatePath);
        
        string result = await engine.CompileRenderStringAsync(templatePath, template, checkoutHeaderDto);
        return result;
    }

}