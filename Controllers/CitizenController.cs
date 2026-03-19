using Microsoft.AspNetCore.Mvc;
using CitizensWebApi.Models;
using CitizensWebApi.Models.DTOs;
using CitizensWebApi.Services;

namespace CitizensWebApi.Controllers
{
    [ApiController]
    [Route("api/citizens")]
    public class CitizenController : ControllerBase
    {

        private readonly FileService _fileService;
        private readonly CitizenService _citizenService;
        private readonly ILogger<CitizenController> _logger;

        public CitizenController(FileService fileService, CitizenService citizenService, ILogger<CitizenController> logger)
        {
            _fileService = fileService;
            _citizenService = citizenService;
            _logger = logger;
        }
        
        // Create Citizen - HTTP POST
        [HttpPost]
        public async Task<IActionResult> CreateCitizen([FromBody] CreateCitizenDto input)
        {
            var citizens = await _fileService.ReadCitizensAsync();
            
            if (citizens.Any(c => c.CI == input.CI))
                return BadRequest(new { message = "Citizen with this CI already exists." });

            var newCitizen = await _citizenService.AssembleCitizenAsync(input);
            citizens.Add(newCitizen);
            await _fileService.WriteCitizensAsync(citizens);

            _logger.LogInformation("Citizen created");
            return CreatedAtAction(nameof(GetCitizenByCi), new { ci = newCitizen.CI }, newCitizen);
        }

        // Update Citizen - HTTP PUT
        [HttpPut("{ci}")]
        public async Task<IActionResult> UpdateCitizen(string ci, [FromBody] UpdateCitizenDto input)
        {
            var citizens = await _fileService.ReadCitizensAsync();
            var citizen = citizens.FirstOrDefault(c => c.CI == ci);

            if (citizen == null)
            {
                _logger.LogWarning("Citizen not found for update: {ci}", ci);
                return NotFound(new { message = "Citizen not found" });
            }

            // Business rule: We only update FirstName and LastName
            citizen.FirstName = input.FirstName;
            citizen.LastName = input.LastName;

            await _fileService.WriteCitizensAsync(citizens);
            _logger.LogInformation("Citizen updated");

            return Ok(citizen);
        }

        // Delete Citizen - HTTP DELETE
        [HttpDelete("{ci}")]
        public async Task<IActionResult> DeleteCitizen(string ci)
        {
            var citizens = await _fileService.ReadCitizensAsync();
            var citizen = citizens.FirstOrDefault(c => c.CI == ci);

            if (citizen == null)
            {
                _logger.LogWarning("Citizen not found for deletion: {ci}", ci);
                return NotFound(new { message = "Citizen not found" });
            }

            citizens.Remove(citizen);
            await _fileService.WriteCitizensAsync(citizens);
            
            _logger.LogInformation("Citizen deleted");
            return Ok(new { message = "Citizen successfully deleted." });
        }

        // Get All Citizens - HTTP GET
        [HttpGet]
        public async Task<IActionResult> GetAllCitizens()
        {
            var citizens = await _fileService.ReadCitizensAsync();
            return Ok(citizens);
        }

        // Get Citizen by CI - HTTP GET /{ci}
        [HttpGet("{ci}")]
        public async Task<IActionResult> GetCitizenByCi(string ci)
        {
            var citizens = await _fileService.ReadCitizensAsync();
            var citizen = citizens.FirstOrDefault(c => c.CI == ci);

            if (citizen == null)
                return NotFound(new { message = "Citizen not found" });

            return Ok(citizen);
        }
    }
}