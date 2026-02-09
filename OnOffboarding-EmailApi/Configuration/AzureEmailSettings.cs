namespace OnOffboarding_EmailApi.Configuration
{
    public class AzureEmailSettings
    {
        public const string SectionName = "ACS"; 
        public string ConnectionString { get; set; } = string.Empty;
        public string SenderAddress { get; set; } = string.Empty;


        //Validerar Inställningarna

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(ConnectionString))
                throw new InvalidOperationException("ACS:ConnectionString är inte konfigurerad");

            if (string.IsNullOrWhiteSpace(SenderAddress))
                throw new InvalidOperationException("ACS:SenderAddress är inte konfigurerad");

            if (!ConnectionString.Contains("endpoint=") || !ConnectionString.Contains("accesskey="))
                throw new InvalidOperationException("ACS:ConnectionString har ogiltigt format");
        }
    }

     
    public class EmailSettings
    {
        public const string SectionName = "EmailSettings";
        public string CtoEmail { get; set; } = string.Empty;
        public string AdminPortalUrl { get; set; } = string.Empty;


        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(CtoEmail))
                throw new InvalidOperationException("EmailSettings:CtoEmail är inte konfigurerad");

            if (string.IsNullOrWhiteSpace(AdminPortalUrl))
                throw new InvalidOperationException("EmailSettings:AdminPortalUrl är inte konfigurerad");

            if (!CtoEmail.Contains("@"))
                throw new InvalidOperationException("EmailSettings:CtoEmail har ogiltigt format");

            if (!AdminPortalUrl.StartsWith("http"))
                throw new InvalidOperationException("EmailSettings:AdminPortalUrl måste börja med http:// eller https://");
        }
    }







}


