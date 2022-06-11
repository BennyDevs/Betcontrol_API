using BetControlAuthentication.Data;
using BetControlAuthentication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BetControlAuthentication.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    //[Route("api/[controller]")] // for backward compatibility
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepo;

        public AuthController(IAuthRepository authRepo)
        {
            _authRepo = authRepo;
        }

        [MapToApiVersion("1.0")]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegister request)
        {
            var response = await _authRepo.Register(
                new User
                {
                    Username = request.Username,
                    Email = request.Email,
                    IsConfirmed = request.IsConfirmed,
                }, request.Password
            );

            return !response.Success ? BadRequest(response) : Ok(response);
        }

        //[HttpPost]
        //[AllowAnonymous]
        //[Route("external-login")]
        //public IActionResult ExternalLogin(string provider, string returnUrl)
        //{
        //    var redirectUrl = $"https://localhost:7291/api/v1/external-auth-callback?returnUrl={returnUrl}";
        //    var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        //    properties.AllowRefresh = true;
        //    return Challenge(properties, provider);
        //}

        [MapToApiVersion("1.0")]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLogin request)
        {
            var response = await _authRepo.Login(request.Email, request.Password);

            return !response.Success ? BadRequest(response) : Ok(response);
        }

    }
}

