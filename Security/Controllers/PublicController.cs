using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Security.Data;

namespace Security.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class PublicController : ControllerBase
{
    private readonly AppDbContext _context;
    public PublicController(AppDbContext context)
    {
        _context=context;
    }

    [HttpGet("courses")]
    public IActionResult GetPublicCourses()

    {
        var courses = _context.Courses
        .Where(c => c.IsPublished == true)
        .Select(c => new {c.Id,c.Title })
        .ToList();
        return Ok(courses);
    }
}