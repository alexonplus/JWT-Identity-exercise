using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Security.Data;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Student")]
public class StudentController : ControllerBase
{
    private readonly AppDbContext _context;

    //  DI
    public StudentController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetMyCourses()
    {
        
        var courses = _context.Courses.ToList();
        return Ok(courses);
    }
}