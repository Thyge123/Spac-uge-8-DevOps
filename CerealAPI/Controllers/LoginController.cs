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

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _usersManager.GetByUsername(model.Username);
            if (user == null)
            {
                return Unauthorized("Invalid username");
            }

            if (!_usersManager.VerifyPassword(user, model.Password))
            {
                return Unauthorized("Invalid password");
            }

            var token = _authHelpers.GenerateJWTToken(user);
            return Ok(new { token });
        }
    }
}
