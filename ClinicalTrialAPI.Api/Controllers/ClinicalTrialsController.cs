using Azure.Core;
using ClinicalTrialAPI.Api.Models;
using ClinicalTrialAPI.Application.Interfaces;
using ClinicalTrialAPI.Application.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ClinicalTrialAPI.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClinicalTrialsController : ControllerBase
    {
        private readonly IClinicalTrialService _service;
        private readonly ILogger<ClinicalTrialsController> _logger;

        public ClinicalTrialsController(IClinicalTrialService service, ILogger<ClinicalTrialsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? status)
        {
            try
            {
                var trials = await _service.GetAllByStatusAsync(status);
                return Ok(trials);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving clinical trials with status {Status}.", status);
                return StatusCode(500, "Unable to retrieve clinical trials. Please try again later.");
            }
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            const long MaxFileSize = 1 * 1024 * 1024; // 1 MB
            var allowedExtensions = new[] { ".json" };

            if (file == null || file.Length == 0)
            {
                return BadRequest("No file was uploaded.");
            }

            if (!allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
            {
                return BadRequest("Only .json files are allowed.");
            }

            if (file.Length > MaxFileSize)
            {
                return BadRequest($"File size must not exceed {MaxFileSize / (1024 * 1024)} MB.");
            }

            using var reader = new StreamReader(file.OpenReadStream());
            var fileContent = await reader.ReadToEndAsync();

            try
            {
                var request = JsonConvert.DeserializeObject<ClinicalTrialRequest>(fileContent);

                var validator = new ClinicalTrialValidator();
                var validationResult = validator.Validate(request.Trial);

                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult.Errors);
                }

                var result = await _service.AddClinicalTrialAsync(request.Trial);
                return Ok(result);
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Invalid JSON format in the uploaded file.");
                return BadRequest("The uploaded file does not contain valid JSON.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the file upload.");
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var trial = await _service.GetByIdAsync(id);
                if (trial == null)
                {
                    _logger.LogWarning("Clinical trial with ID {TrialId} was not found.", id);
                    return NotFound("Clinical trial not found.");
                }
                return Ok(trial);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the clinical trial with ID {TrialId}.", id);
                return StatusCode(500, "Unable to retrieve the clinical trial. Please try again later.");
            }
        }
    }
}
