using OnOffboarding_EmailApi.Models.DTOs;

namespace OnOffboarding_EmailApi.Services
{
    public interface IEmailService
    {
        Task<EmailResponseDto> SendOnboardingEmailAsync(OnboardingEmailDto dto);
        Task<EmailResponseDto> SendOffboardingEmailAsync(OffboardingEmailDto dto);
    }
}
