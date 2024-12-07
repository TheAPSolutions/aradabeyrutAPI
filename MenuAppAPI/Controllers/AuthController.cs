using AradaAPI.Models;
using AradaAPI.Models.DTO.LoginDTO;
using AradaAPI.Repositories.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AradaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly ITokenRepository tokenRepository;

        public AuthController(UserManager<User> userManager, ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO request)
        {
            // Check for Username
            var identityUser = await userManager.FindByNameAsync(request.UserName);

            if (identityUser != null)
            {
                // Check for Password
                var checkPasswordResult = await userManager.CheckPasswordAsync(identityUser, request.Password);

                if (checkPasswordResult)
                {
                    // Retrieve roles
                    var role = await userManager.GetRolesAsync(identityUser);

                    // Generate JWT token
                    var jwtToken = tokenRepository.CreateJWTToken(identityUser, role[0]);

                    // Create response with basic details
                    var response = new LoginResponseDTO()
                    {
                        UserName = request.UserName,
                        Role = role[0],
                        Token = jwtToken,
                    };

                    return Ok(response);
                }
            }

            ModelState.AddModelError("", "Email or Password is Incorrect");
            return ValidationProblem(ModelState);
        }


    }
}
