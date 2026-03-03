using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Security.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Teacher")] //
public class TeacherController : ControllerBase
{
    [HttpGet]
    public IActionResult GetTeacherData()
    {
        return Ok("You are  a teacher.");
    }
}