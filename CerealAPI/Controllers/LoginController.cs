using CerealAPI.Helpers;
using CerealAPI.Manager;
using CerealAPI.Model;
using Microsoft.AspNetCore.Mvc;

namespace CerealAPI.Controllers
{
    public class LoginController : Controller
    {
        private readonly UsersManager _usersManager;
        private readonly AuthHelpers _authHelpers;
        public LoginController(UsersManager usersManager, AuthHelpers authHelpers)
        {
            _usersManager = usersManager;
            _authHelpers = authHelpers;
        }

        // Login user and generate JWT token
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            try
            {
                var user = await _usersManager.GetByUsername(model.Username); // Get user by username
                if (user == null)
                {
                    return Unauthorized("Invalid username"); // Return unauthorized if user not found
                }
                if (!_usersManager.VerifyPassword(user, model.Password)) // Verify password
                {
                    return Unauthorized("Invalid password"); // Return unauthorized if password is incorrect
                }
                var token = _authHelpers.GenerateJWTToken(user); // Generate JWT token
                return Ok(new { token }); // Return token
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while logging in: {ex.Message}");
            }           
        }
    }
}
