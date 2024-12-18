using Azure.Core;
using ClinicalTrialAPI.Api.Models;
using ClinicalTrialAPI.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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
        public async Task<IActionResult> Upload([FromBody] ClinicalTrialRequest request)
        {
            try
            {
                var trial = request.Trial;
                var result = await _service.AddClinicalTrialAsync(trial);
                return Ok(result);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, "The request body was null or missing required properties.");
                return BadRequest("The trial information is required.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while uploading a clinical trial.");
                return StatusCode(500, "Unable to upload the clinical trial. Please try again later.");
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
