using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnOffboarding_EmailApi.Models.DTOs;
using OnOffboarding_EmailApi.Services;

namespace OnOffboarding_EmailApi.Controllers;



[Route("api/[controller]")]
[ApiController]
public class EmailController(IEmailService emailService, ILogger<EmailController> logger) : ControllerBase
{
    private readonly IEmailService _emailService = emailService;
    private readonly ILogger<EmailController> _logger = logger;


    [HttpPost("send-onboarding")]
    [ProducesResponseType(typeof(EmailResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<EmailResponseDto>> SendOnboardingEmail([FromBody] OnboardingEmailDto dto)
    {
        // Validera ModelState (Data Annotations)
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Ogiltig onboarding request: {Errors}",
                string.Join(", ", ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))));
            return BadRequest(ModelState);
        }

        _logger.LogInformation(
            "Mottog onboarding email request för {FirstName} {LastName} från {RequestedBy}",
            dto.FirstName,
            dto.LastName,
            dto.RequestedBy);

        try
        {
            // Skicka email via EmailService
            var result = await _emailService.SendOnboardingEmailAsync(dto);

            if (result.Success)
            {
                _logger.LogInformation("Onboarding email skickat framgångsrikt. Message ID: {MessageId}",
                    result.MessageId);
                return Ok(result);
            }
            else
            {
                _logger.LogError("Kunde inte skicka onboarding email: {Error}", result.ErrorMessage);
                return StatusCode(500, result);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Oväntat fel vid skickning av onboarding email");
            return StatusCode(500, EmailResponseDto.CreateError($"Internt serverfel: {ex.Message}"));
        }
    }




    [HttpPost("send-offboarding")]
    [ProducesResponseType(typeof(EmailResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<EmailResponseDto>> SendOffboardingEmail([FromBody] OffboardingEmailDto dto)
    {
        // Validera ModelState
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Ogiltig offboarding request: {Errors}",
                string.Join(", ", ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))));
            return BadRequest(ModelState);
        }

        _logger.LogInformation(
            "Mottog offboarding email request för {FirstName} {LastName} från {RequestedBy}",
            dto.FirstName,
            dto.LastName,
            dto.RequestedBy);

        try
        {
            // Skicka email via EmailService
            var result = await _emailService.SendOffboardingEmailAsync(dto);

            if (result.Success)
            {
                _logger.LogInformation("Offboarding email skickat framgångsrikt. Message ID: {MessageId}",
                    result.MessageId);
                return Ok(result);
            }
            else
            {
                _logger.LogError("Kunde inte skicka offboarding email: {Error}", result.ErrorMessage);
                return StatusCode(500, result);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Oväntat fel vid skickning av offboarding email");
            return StatusCode(500, EmailResponseDto.CreateError($"Internt serverfel: {ex.Message}"));
        }
    }
}
