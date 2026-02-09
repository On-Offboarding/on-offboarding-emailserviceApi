using Azure;
using Azure.Communication.Email;
using Microsoft.Extensions.Options;
using OnOffboarding_EmailApi.Configuration;
using OnOffboarding_EmailApi.Models.DTOs;
using OnOffboarding_EmailApi.Models.EmailTemplates;

namespace OnOffboarding_EmailApi.Services
{
    public class EmailService :IEmailService
    {
        private readonly EmailClient _emailClient;
        private readonly AzureEmailSettings _acsSettings;
        private readonly EmailSettings _emailSettings;
        private readonly ILogger<EmailService> _logger;
         
        public EmailService(
            IOptions<AzureEmailSettings> acsSettings,
            IOptions<EmailSettings> emailSettings,
            ILogger<EmailService> logger)
        {
            _acsSettings = acsSettings.Value;
            _emailSettings = emailSettings.Value;
            _logger = logger;

            // Validera konfiguration vid uppstart
            _acsSettings.Validate();
            _emailSettings.Validate();

            // Skapa EmailClient med connection string från Azure
            _emailClient = new EmailClient(_acsSettings.ConnectionString);

            _logger.LogInformation("EmailService initialiserad med sender: {Sender}", _acsSettings.SenderAddress);
        }




        public async Task<EmailResponseDto> SendOnboardingEmailAsync(OnboardingEmailDto dto)
        {
            try
            {
                _logger.LogInformation(
                    "Förbereder onboarding email för {FirstName} {LastName} till {CtoEmail}",
                    dto.FirstName,
                    dto.LastName,
                    _emailSettings.CtoEmail);

                // Generera HTML email body från template
                var htmlBody = OnboardingTemplate.GenerateHtml(
                    firstName: dto.FirstName,
                    lastName: dto.LastName,
                    personalNumber: dto.PersonalNumber,
                    department: dto.Department,
                    company: dto.Company,
                    mobileNumber: dto.MobileNumber,
                    employmentDate: dto.EmploymentDate,
                    jobTitle: dto.JobTitle,
                    startDate: dto.StartDate,
                    selectedSystems: dto.SelectedSystems,
                    requestedBy: dto.RequestedBy,
                    adminPortalUrl: _emailSettings.AdminPortalUrl,
                    caseId: dto.CaseId
                );

                // Skapa email subject
                var subject = $" Ny Onboarding: {dto.FirstName} {dto.LastName} ({dto.Company})";

                // Skicka email via Azure Communication Services
                var messageId = await SendEmailAsync(
                    recipientEmail: _emailSettings.CtoEmail,
                    subject: subject,
                    htmlBody: htmlBody
                );

                _logger.LogInformation(
                    "Onboarding email skickat för {FirstName} {LastName}. Message ID: {MessageId}",
                    dto.FirstName,
                    dto.LastName,
                    messageId);

                return EmailResponseDto.CreateSuccess(messageId, _emailSettings.CtoEmail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fel vid skickning av onboarding email för {FirstName} {LastName}",
                    dto.FirstName, dto.LastName);
                return EmailResponseDto.CreateError(ex.Message);
            }
        }



        public async Task<EmailResponseDto> SendOffboardingEmailAsync(OffboardingEmailDto dto)
        {
            try
            {
                _logger.LogInformation(
                    "Förbereder onboarding email för {FirstName} {LastName} till {CtoEmail}",
                    dto.FirstName,
                    dto.LastName,
                    _emailSettings.CtoEmail);

                // Generera HTML email body från template
                var htmlBody = OffboardingTemplate.GenerateHtml(
                    firstName: dto.FirstName,
                    lastName: dto.LastName,
                    personalNumber: dto.PersonalNumber,
                    department: dto.Department,
                    company: dto.Company,
                    mobileNumber: dto.MobileNumber,
                    employmentDate: dto.EmploymentDate,
                    jobTitle: dto.JobTitle,
                    startDate: dto.StartDate,
                    selectedSystems: dto.SelectedSystems,
                    requestedBy: dto.RequestedBy,
                    adminPortalUrl: _emailSettings.AdminPortalUrl,
                    caseId: dto.CaseId
                );

                // Skapa email subject med urgency indicator
                var subject = $"Offboarding för personal: {dto.FirstName} {dto.LastName} ({dto.Company})";

                // Skicka email via Azure Communication Services
                var messageId = await SendEmailAsync(
                    recipientEmail: _emailSettings.CtoEmail,
                    subject: subject,
                    htmlBody: htmlBody
                );

                _logger.LogInformation(
                    "Offboarding email skickat för {FirstName} {LastName}. Message ID: {MessageId}",
                    dto.FirstName,
                    dto.LastName,
                    messageId);

                return EmailResponseDto.CreateSuccess(messageId, _emailSettings.CtoEmail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fel vid skickning av offboarding email för {FirstName} {LastName}",
                    dto.FirstName, dto.LastName);
                return EmailResponseDto.CreateError(ex.Message);
            }
        }

        private async Task<string> SendEmailAsync(string recipientEmail, string subject, string htmlBody)
        {
            try
            {
                // Skapa email message
                var emailMessage = new EmailMessage(
                    senderAddress: _acsSettings.SenderAddress,
                    content: new EmailContent(subject)
                    {
                        Html = htmlBody
                    },
                    recipients: new EmailRecipients(new List<EmailAddress>
                    {
                        new EmailAddress(recipientEmail)
                    })
                );

                _logger.LogDebug("Skickar email till {Recipient} med subject: {Subject}", recipientEmail, subject);

                // Skicka email via Azure Communication Services
                EmailSendOperation emailSendOperation = await _emailClient.SendAsync(WaitUntil.Completed, emailMessage);

                //// Vänta på att emailet ska skickas (timeout efter 2 minuter)
                //var timeout = TimeSpan.FromMinutes(2);
                //await emailSendOperation.WaitForCompletionAsync(timeout);

                // Kontrollera status
                if (emailSendOperation.HasValue)
                {
                    var status = emailSendOperation.Value.Status;
                    _logger.LogInformation("Email status: {Status}, Message ID: {MessageId}",
                        status,
                        emailSendOperation.Id);

                    if (status == EmailSendStatus.Succeeded)
                    {
                        return emailSendOperation.Id;
                    }
                    else
                    {
                        throw new Exception($"Email skickades inte. Status: {status}");
                    }
                }
                else
                {
                    throw new Exception("Inget response från Azure Communication Services");
                }
            }
            catch (RequestFailedException ex)
            {
                _logger.LogError(ex, "Azure Communication Services request misslyckades: {ErrorCode} - {Message}",
                    ex.ErrorCode, ex.Message);
                throw new Exception($"Azure email fel: {ex.ErrorCode} - {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Oväntat fel vid skickning av email");
                throw;
            }
        }
    }
}
