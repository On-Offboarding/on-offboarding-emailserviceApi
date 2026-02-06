namespace OnOffboarding_EmailApi.Models.DTOs
{
    public class EmailResponseDto
    {
        public bool Success { get; set; }

        /// Meddelande om resultatet (t.ex. "Email skickat", "Fel vid skickning")
        public string Message { get; set; } = string.Empty;

        ///// Azure Message ID (för spårning i Azure Communication Services)
        public string? MessageId { get; set; }


        /// Tidsstämpel när emailet skickades
        public DateTime SentAt { get; set; }

        /// Mottagarens email-adress
        public string? RecipientEmail { get; set; }

        /// Eventuellt felmeddelande om något gick fel
        public string? ErrorMessage { get; set; }

 
        /// Skapar ett success-response
        public static EmailResponseDto CreateSuccess(string messageId, string recipientEmail)
        {
            return new EmailResponseDto
            {
                Success = true,
                Message = "Email har skickats",
                MessageId = messageId,
                SentAt = DateTime.UtcNow,
                RecipientEmail = recipientEmail
            };
        }

        /// Skapar ett error-response
        public static EmailResponseDto CreateError(string errorMessage)
        {
            return new EmailResponseDto
            {
                Success = false,
                Message = "Kunde inte skicka email",
                ErrorMessage = errorMessage,
                SentAt = DateTime.UtcNow
            };
        }
    }
}
