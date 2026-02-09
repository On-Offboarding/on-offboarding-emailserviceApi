using System.ComponentModel.DataAnnotations;

namespace OnOffboarding_EmailApi.Models.DTOs
{
    public class OnboardingEmailDto
    {


        [Required(ErrorMessage = "Förnamn är obligatoriskt")]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;


        [Required(ErrorMessage = "Efternamn är obligatoriskt")]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Personnummer är obligatoriskt")]
        [RegularExpression(@"^\d{12}$", ErrorMessage = "Personnummer måste vara 12 siffror")]
        public string PersonalNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Avdelning är obligatoriskt")]
        [StringLength(100)]
        public string Department { get; set; } = string.Empty;


        [Required(ErrorMessage = "Företag är obligatoriskt")]
        [RegularExpression("^(Finansia|AgeraPay)$", ErrorMessage = "Företag måste vara Finansia eller AgeraPay")]
        public string Company { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mobilnummer är obligatoriskt")]
        [Phone(ErrorMessage = "Ogiltigt mobilnummer")]
        public string MobileNumber { get; set; } = string.Empty;


        [Required(ErrorMessage = "Anställningsdag är obligatoriskt")]
        public DateTime EmploymentDate { get; set; }


        [Required(ErrorMessage = "Tjänstetitel är obligatoriskt")]
        [StringLength(150)]
        public string JobTitle { get; set; } = string.Empty;


        [Required(ErrorMessage = "Startdatum är obligatoriskt")]
        public DateTime StartDate { get; set; }


        [Required(ErrorMessage = "Minst ett system måste väljas")]
        [MinLength(1, ErrorMessage = "Minst ett system måste väljas")]
        public List<string> SelectedSystems { get; set; } = new();


        [Required(ErrorMessage = "Begärd av är obligatoriskt")]
        [EmailAddress(ErrorMessage = "Ogiltig email-adress")]
        public string RequestedBy { get; set; } = string.Empty;


        public Guid? CaseId { get; set; }
    }
}
