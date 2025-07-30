using Infrastructure.Configurations;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using Org.BouncyCastle.Utilities.Net;
using Saga.Orchestrator.Services.Interfaces;
using Shared.DTOs;
using System.Threading;
using MailKit.Net.Smtp;
using MimeKit;

using ILogger = Serilog.ILogger;
namespace Saga.Orchestrator.Services
{
    public class BookingEmailService : EmailTemplateService, IEmailTemplateService
    {
        private readonly SMTPEmailSetting _smtpSettings;
        private readonly ILogger _logger;
        private readonly SmtpClient _smtpClient;
        private readonly IConfiguration _configuration;
        public BookingEmailService(IOptions<SMTPEmailSetting> smtpSettings, ILogger logger,IConfiguration configuration)
        {
            _smtpSettings = smtpSettings.Value;
            _logger = logger;
            this._smtpClient = new SmtpClient();
            this._configuration = configuration;
        }
        public async Task SendBookingRoomConfirmationEmail(BookingRoomResponseDTO bookingRoom)
        {
            try
            {
                _logger.Information($"Sending room booking confirmation email for booking ID: {bookingRoom.Id}");

                // Read email template
                string emailTemplate = ReadEmailTemplateContent("RoomBookingConfirmation","html");

                // Replace placeholders with actual data
                var emailBody = ReplaceRoomBookingPlaceholders(emailTemplate, bookingRoom);

                // Send email
                await SendEmailAsync(bookingRoom.Email, "Room Booking Confirmation", emailBody);

                _logger.Information($"Room booking confirmation email sent successfully for booking ID: {bookingRoom.Id}");
            }
            catch (Exception ex)
            {
                _logger.Error($"Error sending room booking confirmation email: {ex.Message}");
            }
        }
        private string ReplaceRoomBookingPlaceholders(string template, BookingRoomResponseDTO booking)
        {
            var result = template;

            // Basic replacements
            result = result.Replace("{{UserName}}", booking.FullName ?? "Valued Customer");
            result = result.Replace("{{BookingRoomId}}", booking.Id.ToString());
            result = result.Replace("{{CheckInDate}}", booking.CheckIn?.ToString("dd/MM/yyyy") ?? "");
            result = result.Replace("{{CheckOutDate}}", booking.CheckOut?.ToString("dd/MM/yyyy") ?? "");
            result = result.Replace("{{TotalPrice}}", booking.PriceTotal.ToString("#,##0"));
            result = result.Replace("{{NumberOfPeople}}", booking.NumberOfPeople.ToString());
            result = result.Replace("{{Status}}", booking.Status);
            result = result.Replace("{{CurrentYear}}", DateTime.Now.Year.ToString());

            // Generate room details table rows
            var roomDetailsList = new System.Text.StringBuilder();
            foreach (var room in booking.DetailBookingRooms)
            {
                roomDetailsList.AppendLine("<tr>");
                roomDetailsList.AppendLine($"<td>{room.Room?.Name ?? "Room"}</td>");
                roomDetailsList.AppendLine($"<td>{room.Adults}</td>");
                roomDetailsList.AppendLine($"<td>{room.Children}</td>");
                roomDetailsList.AppendLine($"<td>${room.Price}</td>");
                roomDetailsList.AppendLine("</tr>");
            }
            result = result.Replace("{{RoomDetailsList}}", roomDetailsList.ToString());

            // URL for booking details (replace with your actual frontend URL)
            // Do hiện tại FE ko còn phát triển nữa nên sẽ làm sau
            result = result.Replace("{{BookingUrl}}", $"https://yourwebsite.com/bookings/room/{booking.Id}");

            return result;
        }
        private string ReplaceTourBookingPlaceholders(string template, BookingTourCustomResponseDTO booking)
        {
            var result = template;

            // Basic replacements
            result = result.Replace("{{UserName}}", booking.FullName ?? "Valued Customer");
            result = result.Replace("{{BookingTourId}}", booking.Id.ToString());
            result = result.Replace("{{TourName}}", booking.Schedule?.Tour?.Name ?? "Tour Package");
            result = result.Replace("{{DepartureDate}}", booking.Schedule?.DateStart.ToString("dd/MM/yyyy") ?? "");
            result = result.Replace("{{ReturnDate}}", booking.Schedule?.DateEnd.ToString("dd/MM/yyyy") ?? "");
            result = result.Replace("{{Seats}}", booking.Seats.ToString());
            result = result.Replace("{{TotalPrice}}", booking.PriceTotal.ToString("#,##0"));
            result = result.Replace("{{Status}}", booking.Status);
            result = result.Replace("{{CurrentYear}}", DateTime.Now.Year.ToString());

            // Tour specific details
            result = result.Replace("{{TourDetail}}", booking.Schedule?.Tour?.Detail ?? "");
            result = result.Replace("{{TourExpect}}", "Experience the wonders of your destination with our expert guides.");
            //result = result.Replace("{{TourImageUrl}}", booking.Schedule?.Tour?.ImagesList?.FirstOrDefault() ?? "https://booking-cloud-storage.s3.amazonaws.com/jack-ward-rknrvCrfS1k-unsplash-scaled.jpg");

            // Generate traveller details table rows
            var travellersList = new System.Text.StringBuilder();
            if (booking.Travellers != null)
            {
                foreach (var traveller in booking.Travellers)
                {
                    travellersList.AppendLine("<tr>");
                    travellersList.AppendLine($"<td>{traveller.Fullname}</td>");
                    travellersList.AppendLine($"<td>{traveller.Gender}</td>");
                    travellersList.AppendLine($"<td>{traveller.Age}</td>");
                    travellersList.AppendLine($"<td>{traveller.Phone}</td>");
                    travellersList.AppendLine("</tr>");
                }
            }
            result = result.Replace("{{TravellersList}}", travellersList.ToString());

            // URL for booking details (replace with your actual frontend URL)
            result = result.Replace("{{BookingUrl}}", $"https://yourwebsite.com/bookings/tour/{booking.Id}");

            return result;
        }
        private async Task SendEmailAsync(string to, string subject, string body)
        {
            _logger.Information("Password : {0}",_configuration.GetSection("SMTPEmailSettings:Password"));
            var emailMessage = new MimeMessage
            {
                Sender = new MailboxAddress(_smtpSettings.DisplayName, _smtpSettings.From),
                Subject = subject,
                Body = new BodyBuilder()
                {
                    HtmlBody = body,
                }.ToMessageBody(),
            };
            emailMessage.To.Add(MailboxAddress.Parse(to));
            try
            {
               await  _smtpClient.ConnectAsync(_smtpSettings.Host, _smtpSettings.Port, _smtpSettings.UseSsl);
               await  _smtpClient.AuthenticateAsync(_smtpSettings.UserName, _smtpSettings.Password);
               await _smtpClient.SendAsync(emailMessage);
               await _smtpClient.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
               await _smtpClient.DisconnectAsync(true);
               _smtpClient.Dispose();
            }
        }
        public async Task SendBookingTourConfirmationEmail(BookingTourCustomResponseDTO bookingTour)
        {
            try
            {
                _logger.Information($"Sending tour booking confirmation email for booking ID: {bookingTour.Id}");

                // Read email template
                string emailTemplate = ReadEmailTemplateContent("TourBookingConfirmation","html");

                // Replace placeholders with actual data
                var emailBody = ReplaceTourBookingPlaceholders(emailTemplate,bookingTour);

                // Send email
                await SendEmailAsync(bookingTour.Email, "Tour Booking Confirmation", emailBody);

                _logger.Information($"Tour booking confirmation email sent successfully for booking ID: {bookingTour.Id}");
            }
            catch (Exception ex)
            {
                _logger.Error($"Error sending tour booking confirmation email: {ex.Message}");
            }
        }
    }
}
