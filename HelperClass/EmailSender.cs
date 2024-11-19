using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using twitterclone.HelperClass;
using twitterclone.DTOs;
public class EmailSender{
   
   
   private readonly IConfiguration _configuration;

   public EmailSender( IConfiguration configuration){
    
     _configuration = configuration;
   }

  public async Task SendEmailAsync(EmailDto emailDto)
{

 var smtpClient = new SmtpClient
        {
            Host = _configuration["SmtpSettings:Server"],
            Port = int.Parse(_configuration["SmtpSettings:Port"]),
            EnableSsl = true,
            Credentials = new NetworkCredential(
                _configuration["SmtpSettings:Username"],
                _configuration["SmtpSettings:Password"]
            )
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(_configuration["SmtpSettings:SenderEmail"], _configuration["SmtpSettings:SenderName"]),
            Subject = emailDto.Subject,
            Body = emailDto.Body,
            IsBodyHtml = true
        };

        mailMessage.To.Add(emailDto.ToEmail);

        await smtpClient.SendMailAsync(mailMessage);

} 



}