using JwtProject;
using JwtProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JwtProject.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : Controller
{
    private readonly JwtContext _jwtContext;
    private readonly IConfiguration _configuration;

    public AuthController(JwtContext jwtC, IConfiguration configuration)
    {
        _jwtContext= jwtC;
        _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<ActionResult<string>> RegisterUser(User request)
    {
        try
        {
            var user = _jwtContext.Users.Add(new User
            {
                Nickname = request.Nickname,
                Password = request.Password
            });
            await _jwtContext.SaveChangesAsync();

            var jwt = JwtGenerator.GenerateJwt(user.Entity, _configuration.GetValue<string>("TokenKey")!, DateTime.UtcNow.AddMinutes(5));

            HttpContext.Session.SetInt32("id", user.Entity.Id);

            return Created("token", jwt);
        }
        catch (Exception ex)
        {
            return BadRequest($"Registration failed: {ex.Message}");
        }
    }


    [HttpPost("login")]
    public async Task<ActionResult<string>> Login(User request)
    {
        try
        {
            var user = await _jwtContext.Users
                .SingleOrDefaultAsync(x => x.Nickname == request.Nickname && x.Password == request.Password);

            if (user == null)
            {
                return BadRequest("Invalid credentials");
            }

            var jwt = JwtGenerator.GenerateJwt(user, _configuration.GetValue<string>("TokenKey")!, DateTime.UtcNow.AddMinutes(5));

            return Created("token", jwt);
        }
        catch (Exception ex)
        {
            return BadRequest($"Login failed: {ex.Message}");
        }
    }

}