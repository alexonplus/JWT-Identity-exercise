using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Security.Data;
using Security.Models;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = "Bearer", Roles = "Student")]
public class StudentController : ControllerBase
{
    private readonly AppDbContext _context;

    public StudentController(AppDbContext context)
    {
        _context = context;
    }

    // POST /api/student/courses/{courseId}/— enroll in a course
    [HttpPost("courses/{courseId}/enroll")]
    public async Task<IActionResult> Enroll(int courseId)
    {
        var studentId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        // Check if course exists
        var course = await _context.Courses.FindAsync(courseId);
        if (course == null) return NotFound("Course not found");

        // Check if student is already enrolled
        var existing = _context.Enrollments
            .FirstOrDefault(e => e.CourseId == courseId && e.StudentId == studentId);
        if (existing != null)
            return BadRequest("Already enrolled");

        var enrollment = new Enrollment
        {
            CourseId = courseId,
            StudentId = studentId!,
            Status = EnrollmentStatus.Pending
        };

        _context.Enrollments.Add(enrollment);
        await _context.SaveChangesAsync();
        return Ok("Enrollment created, status: Pending");
    }

    // GET /api/student/courses/{courseId} — get course if Approved
    [HttpGet("courses/{courseId}")]
    public async Task<IActionResult> GetCourse(int courseId)
    {
        var studentId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var course = await _context.Courses.FindAsync(courseId);
        if (course == null) return NotFound();
        

        if (!course.IsPublished)
            return Forbid(); // 403

        // Check enrollment and status
        var enrollment = _context.Enrollments
            .FirstOrDefault(e => e.CourseId == courseId && e.StudentId == studentId);

        if (enrollment == null || enrollment.Status != EnrollmentStatus.Approved)
            return Forbid(); // 403 — not 404!

        return Ok(course);
    }
}