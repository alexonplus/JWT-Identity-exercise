using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Security.Models;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = Roles.Admin)]
public class AdminController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;

    public AdminController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    // GET /api/admin/users — список всех пользователей с ролями
    [HttpGet("users")]
    public async Task<IActionResult> GetUsers()
    {
        var users = _userManager.Users.ToList();

        var result = new List<object>();
        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            result.Add(new { user.Id, user.Email, Roles = roles });
        }

        return Ok(result);
    }

    // POST /api/admin/users/{userId}/role — назначить роль пользователю
    [HttpPost("users/{userId}/role")]
    public async Task<IActionResult> SetRole(string userId, [FromBody] SetRoleDto dto)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return NotFound("User not found");

        // Проверяем что роль существует
        var validRoles = new[] { Roles.Admin, Roles.Teacher, Roles.Student };
        if (!validRoles.Contains(dto.Role))
            return BadRequest("Invalid role");

        // Убираем старые роли и ставим новую
        var currentRoles = await _userManager.GetRolesAsync(user);
        await _userManager.RemoveFromRolesAsync(user, currentRoles);
        await _userManager.AddToRoleAsync(user, dto.Role);

        return Ok($"Role {dto.Role} assigned to {user.Email}");
    }
}