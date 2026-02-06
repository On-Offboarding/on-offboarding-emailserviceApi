namespace OnOffboarding_EmailApi.Models.EmailTemplates
{
    public class OnboardingTemplate
    {

        public static string GenerateHtml(string firstName,string lastName,string personalNumber,string department,string company,string mobileNumber,DateTime employmentDate,string jobTitle,DateTime startDate,List<string> selectedSystems,string requestedBy,string adminPortalUrl,Guid? caseId = null)
        {
            // Generera HTML-lista av system med checkboxar (visuellt)
            var systemsHtml = string.Join("", selectedSystems.Select(s =>
                $"<li style='margin-bottom: 8px;'><span style='color: #4CAF50; margin-right: 8px;'>✓</span>{s}</li>"
            ));

            // Case ID del (om det finns)
            var caseIdSection = caseId.HasValue
                ? $@"
                <tr>
                    <td style='padding: 8px; font-weight: bold; color: #555; border-bottom: 1px solid #e0e0e0;'>Case ID:</td>
                    <td style='padding: 8px; border-bottom: 1px solid #e0e0e0;'>{caseId.Value}</td>
                </tr>"
                : string.Empty;

            // Huvudtemplate med professional design
            return $@"

            <!DOCTYPE html>
            <html lang='sv'>
            <head>
                <meta charset='UTF-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                <title>Ny Onboarding Begäran</title>
            </head>
            <body style='margin: 0; padding: 0; font-family: Arial, sans-serif; background-color: #f4f4f4;'>
                <div style='max-width: 700px; margin: 20px auto; background-color: #ffffff; border-radius: 8px; overflow: hidden; box-shadow: 0 2px 8px rgba(0,0,0,0.1);'>
        
                    <!-- Header -->
                    <div style='background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); padding: 30px; text-align: center;'>
                        <h1 style='margin: 0; color: #ffffff; font-size: 28px; font-weight: 600;'>
                            Ny Onboarding Begäran
                        </h1>
                        <p style='margin: 10px 0 0 0; color: #e0e7ff; font-size: 16px;'>
                            En chef har begärt onboarding för ny personal
                        </p>
                    </div>

                    <!-- Content -->
                    <div style='padding: 30px;'>
            
                        <!-- Intro Text -->
                        <p style='font-size: 16px; color: #333; line-height: 1.6; margin-bottom: 25px;'>
                            Hej CTO,<br><br>
                            En ny onboarding-begäran har skapats i systemet. Nedan hittar du all information och de systemåtkomster som behöver konfigureras.
                        </p>

                        <!-- Employee Information Table -->
                        <h2 style='color: #667eea; font-size: 20px; margin-bottom: 15px; border-bottom: 2px solid #667eea; padding-bottom: 8px;'>
                            Personinformation
                        </h2>
                        <table style='width: 100%; border-collapse: collapse; margin-bottom: 30px;'>
                            <tr>
                                <td style='padding: 8px; font-weight: bold; color: #555; border-bottom: 1px solid #e0e0e0;'>Namn:</td>
                                <td style='padding: 8px; border-bottom: 1px solid #e0e0e0;'>{firstName} {lastName}</td>
                            </tr>
                            <tr>
                                <td style='padding: 8px; font-weight: bold; color: #555; border-bottom: 1px solid #e0e0e0;'>Personnummer:</td>
                                <td style='padding: 8px; border-bottom: 1px solid #e0e0e0;'>{personalNumber}</td>
                            </tr>
                            <tr>
                                <td style='padding: 8px; font-weight: bold; color: #555; border-bottom: 1px solid #e0e0e0;'>Tjänstetitel:</td>
                                <td style='padding: 8px; border-bottom: 1px solid #e0e0e0;'>{jobTitle}</td>
                            </tr>
                            <tr>
                                <td style='padding: 8px; font-weight: bold; color: #555; border-bottom: 1px solid #e0e0e0;'>Avdelning:</td>
                                <td style='padding: 8px; border-bottom: 1px solid #e0e0e0;'>{department}</td>
                            </tr>
                            <tr>
                                <td style='padding: 8px; font-weight: bold; color: #555; border-bottom: 1px solid #e0e0e0;'>Företag:</td>
                                <td style='padding: 8px; border-bottom: 1px solid #e0e0e0;'>{company}</td>
                            </tr>
                            <tr>
                                <td style='padding: 8px; font-weight: bold; color: #555; border-bottom: 1px solid #e0e0e0;'>Mobilnummer:</td>
                                <td style='padding: 8px; border-bottom: 1px solid #e0e0e0;'>{mobileNumber}</td>
                            </tr>
                            <tr>
                                <td style='padding: 8px; font-weight: bold; color: #555; border-bottom: 1px solid #e0e0e0;'>Anställningsdag:</td>
                                <td style='padding: 8px; border-bottom: 1px solid #e0e0e0;'>{employmentDate:yyyy-MM-dd}</td>
                            </tr>
                            <tr>
                                <td style='padding: 8px; font-weight: bold; color: #555; border-bottom: 1px solid #e0e0e0;'>Startdatum:</td>
                                <td style='padding: 8px; border-bottom: 1px solid #e0e0e0;'>{startDate:yyyy-MM-dd}</td>
                            </tr>
                            <tr>
                                <td style='padding: 8px; font-weight: bold; color: #555; border-bottom: 1px solid #e0e0e0;'>Begärd av:</td>
                                <td style='padding: 8px; border-bottom: 1px solid #e0e0e0;'>{requestedBy}</td>
                            </tr>
                            {caseIdSection}
                      
                        </table>

                        <!-- Systems Access -->
                        <h2 style='color: #667eea; font-size: 20px; margin-bottom: 15px; border-bottom: 2px solid #667eea; padding-bottom: 8px;'>
                            Systemåtkomster att konfigurera
                        </h2>
                        <div style='background-color: #f8f9fa; border-left: 4px solid #4CAF50; padding: 15px; border-radius: 4px; margin-bottom: 30px;'>
                            <p style='margin: 0 0 10px 0; font-weight: bold; color: #333;'>
                                Följande system ska konfigureras ({selectedSystems.Count} st):
                            </p>
                            <ul style='margin: 0; padding-left: 20px; list-style-type: none;'>
                                {systemsHtml}
                            </ul>
                        </div>

                        <!-- Action Button -->
                        <div style='text-align: center; margin: 30px 0;'>
                            <a href='{adminPortalUrl}' 
                                style='display: inline-block; background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: #ffffff; text-decoration: none; padding: 14px 30px; border-radius: 6px; font-size: 16px; font-weight: 600; box-shadow: 0 4px 6px rgba(102, 126, 234, 0.3);'>
                                Öppna Adminportalen
                            </a>
                        </div>

                    </div>

                    <!-- Footer -->
                    <div style='background-color: #f8f9fa; padding: 20px; text-align: center; border-top: 1px solid #e0e0e0;'>
                        <p style='margin: 0; color: #6c757d; font-size: 13px;'>
                            Detta är ett automatiskt email från On/Offboarding-systemet<br>
                            {company} © {DateTime.UtcNow.Year}
                        </p>
                    </div>
                </div>
            </body>
            </html>";
        }
    }
}
