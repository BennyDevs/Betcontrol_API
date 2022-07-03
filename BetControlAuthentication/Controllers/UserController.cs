using System.Security.Claims;
using BetControlAuthentication.Data;
using BetControlAuthentication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BetControlAuthentication.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    [EnableCors("ReactPolicy")]
    public class UserController : ControllerBase
    {
        private readonly DataContext _context;

        public UserController(DataContext context)
        {
            _context = context;
        }

        private int GetUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        private async Task<User?> GetUser() => await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());


        [MapToApiVersion("1.0")]
        [HttpGet("userprofile")]
        public async Task<ActionResult<User>> GetUserProfile()
        {
            User? user = await GetUser();

            if (user is not null)
            {
                UserProfile? userProfile = new UserProfile
                {
                    Email = user.Email,
                    Username = user.Username,
                    Bio = user.Bio
                };

                try
                {
                    return Ok(userProfile);
                }
                catch (DbUpdateException)
                {
                    return BadRequest("Was not able to retrieve the users information");
                }
            }
            return BadRequest("No user was found.");
        }
    }
}

