using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Security.Data;
using Security.Models;
using System.Security.Claims;

namespace Security.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = "Bearer", Roles = "Teacher")]
public class TeacherController : ControllerBase
{
    private readonly AppDbContext _context;

    public TeacherController(AppDbContext context)
    {
        _context = context;
    }

    // POST /api/teacher/courses — create a new course
    [HttpPost("courses")]
    public async Task<IActionResult> CreateCourse([FromBody] CreateCourseDto dto)
    {
        var teacherId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var course = new Course
        {
            Title = dto.Title,
            TeacherId = teacherId!,
            IsPublished = false
        };

        _context.Courses.Add(course);
        await _context.SaveChangesAsync();
        return Ok(course);
    }

    // PATCH /api/teacher/courses/{courseId}/ courseId for publish 
    [HttpPatch("courses/{courseId}/publish")]
    public async Task<IActionResult> PublishCourse(int courseId)
    {
        var teacherId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var course = await _context.Courses.FindAsync(courseId);
        if (course == null) return NotFound();

    
        if (course.TeacherId != teacherId)
            return Forbid(); // 403

        course.IsPublished = true;

        // Auto-approve все Pending enrollments for this course
        var pendingEnrollments = _context.Enrollments
            .Where(e => e.CourseId == courseId && e.Status == EnrollmentStatus.Pending)
            .ToList();

        foreach (var enrollment in pendingEnrollments)
            enrollment.Status = EnrollmentStatus.Approved;

        await _context.SaveChangesAsync();
        return Ok("Course published and enrollments approved");
    }
}