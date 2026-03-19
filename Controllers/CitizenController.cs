using Microsoft.AspNetCore.Mvc;
using CitizensWebApi.Models;
using CitizensWebApi.Models.DTOs;

namespace CitizensWebApi.Controllers
{
    [ApiController]
    [Route("api/citizens")]
    public class CitizenController : ControllerBase
    {
        // Create Citizen - HTTP POST
        [HttpPost]
        public IActionResult CreateCitizen([FromBody] CreateCitizenDto input)
        {
            // Assign blood type, call external API, save to CSV
            return Ok(new { message = "Citizen created endpoint reached" });
        }

        // Update Citizen - HTTP PUT
        [HttpPut("{ci}")]
        public IActionResult UpdateCitizen(string ci, [FromBody] UpdateCitizenDto input)
        {
            // Check for existence, update only FirstName and LastName, save to CSV
            return Ok(new { message = $"Update endpoint reached for CI: {ci}" });
        }

        // Delete Citizen - HTTP DELETE
        [HttpDelete("{ci}")]
        public IActionResult DeleteCitizen(string ci)
        {
            // Check for existence, remove from CSV
            return Ok(new { message = $"Delete endpoint reached for CI: {ci}" });
        }

        // Get All Citizens - HTTP GET
        [HttpGet]
        public IActionResult GetAllCitizens()
        {
            // Read all CSV citizens
            return Ok(new List<Citizen>());
        }

        // Get Citizen by CI - HTTP GET /{ci}
        [HttpGet("{ci}")]
        public IActionResult GetCitizenByCi(string ci)
        {
            // Search for a citizen by CI number in the CSV
            return Ok(new { message = $"Get by CI endpoint reached for CI: {ci}" });
        }
    }
}