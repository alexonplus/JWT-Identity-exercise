using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Security.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Student")] // Вот этот замок! Пустит только тех, у кого в токене роль Teacher
public class StudentController : ControllerBase
{
    [HttpGet]
    public IActionResult GetStudentrData()
    {
        return Ok("Student  student");
    }
}