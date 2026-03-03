using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Security.Controllers;

[ApiController]
[Route("api/[controller]")]
 // Вот этот замок! Пустит только тех, у кого в токене роль Teacher
public class PublicController : ControllerBase
{
    [HttpGet]
    public IActionResult GetTeacherData()
    {
        return Ok("public public");
    }
}