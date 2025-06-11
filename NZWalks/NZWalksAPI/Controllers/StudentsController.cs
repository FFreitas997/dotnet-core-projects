using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NZWalksAPI.Controllers
{
    // This controller is responsible for handling student-related requests
    //https://localhost:7175/api/students
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController(ILogger<StudentsController> logger) : ControllerBase
    {
        private static readonly string[] Students = ["Alice", "Bob", "Charlie"];

        [HttpGet]
        public IActionResult GetAllStudents()
        {
            logger.LogInformation("The client has requested all students.");

            return Ok(Students);
        }
    }
}