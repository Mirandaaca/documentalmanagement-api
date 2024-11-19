using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Project.Application.DTOs.Infraestructure.Persistence.Estudiante;
using Project.Application.Interfaces.Persistence;

namespace Project.Persistence.Services
{
  public class SendEmailService : ISendEmailService
  {
    private readonly IConfiguration _configuration;
    public SendEmailService(IConfiguration configuration)
    {
      _configuration = configuration;
    }
    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
      var emailConfig = _configuration.GetSection("EmailSettings");
      string fromEmail = emailConfig["Email"];
      string password = emailConfig["Password"];
      string host = emailConfig["Host"];
      int port = int.Parse(emailConfig["Port"]);
      bool enableSSL = bool.Parse(emailConfig["EnableSSL"]);

      using var client = new SmtpClient(host, port)
      {
        EnableSsl = enableSSL,
        UseDefaultCredentials = false,
        Credentials = new NetworkCredential(fromEmail, password)
      };

      using var mailMessage = new MailMessage
      {
        From = new MailAddress(fromEmail),
        Subject = subject,
        IsBodyHtml = true,
        Body = htmlMessage
      };

      mailMessage.To.Add(email);

      await client.SendMailAsync(mailMessage);
    }
    public string GenerarMensajeEmail(SeguimientoEstudianteDTO estudiante)
    {
      var builder = new StringBuilder();
      builder.AppendLine($"<h1>Hola, {estudiante.Nombre} {estudiante.Apellido}</h1>");
      builder.AppendLine("<p>Hemos identificado que tienes documentos pendientes por presentar:</p>");
      builder.AppendLine("<ul>");

      foreach (var documento in estudiante.DocumentosFaltantes)
      {
        string estado = documento.EstaExpirado ? "Expirado" : "Por Vencer";
        string diasInfo = documento.EstaExpirado
            ? $"{Math.Abs(documento.DiasRestantes)} días de retraso"
            : $"quedan {documento.DiasRestantes} días para presentarlo";

        builder.AppendLine($"<li>{documento.NombreDocumento}: {estado} ({diasInfo})</li>");
      }

      builder.AppendLine("</ul>");

      if (estudiante.DiasParaProximaExpiracion.HasValue)
      {
        builder.AppendLine($"<p>El documento más próximo a expirar debe presentarse en {estudiante.DiasParaProximaExpiracion.Value} días ({estudiante.FechaProximaExpiracion:dd/MM/yyyy}).</p>");
      }
      else
      {
        builder.AppendLine("<p>No hay fechas próximas a expirar actualmente.</p>");
      }

      builder.AppendLine("<p>Por favor, asegúrate de presentar los documentos antes de la fecha límite correspondiente.</p>");
      builder.AppendLine("<p>Atentamente,<br>El Departamento de Admisiones</p>");

      return builder.ToString();
    }


  }
}
