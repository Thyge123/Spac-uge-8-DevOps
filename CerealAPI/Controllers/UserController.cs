using CerealAPI.Manager;
using CerealAPI.Model;
using Microsoft.AspNetCore.Mvc;

namespace CerealAPI.Controllers
{
    [ApiController]
    public class UserController : Controller
    {
        // Users manager to handle business logic
        private readonly UsersManager _usersManager;

        public UserController(UsersManager usersManager)
        {
            _usersManager = usersManager;
        }

        // Get All users
        [HttpGet]
        [Route("api/users")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return Ok(await _usersManager.GetAllAsync()); // Return all users 
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving users: {ex.Message}");
            }
        }

        // Get user by ID
        [HttpGet]
        [Route("api/users/{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var user = await _usersManager.Get(id); 
                if (user == null)
                {
                    return NotFound();
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving user: {ex.Message}");
            }
        }

        // Get user by username
        [HttpGet]
        [Route("api/users/username/{username}")]
        public async Task<IActionResult> GetByUsername(string username)
        {
            try
            {
                var user = await _usersManager.GetByUsername(username);
                if (user == null)
                {
                    return NotFound();
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving user: {ex.Message}");
            }
        }

        // Get user by role
        [HttpGet]
        [Route("api/users/role/{role}")]
        public async Task<IActionResult> GetByRole(string role)
        {
            try
            {
                var user = await _usersManager.GetByRole(role);
                if (user == null)
                {
                    return NotFound();
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving user: {ex.Message}");
            }
        }

        // Create user
        [HttpPost]
        [Route("api/users")]
        public async Task<IActionResult> Create([FromBody] User user)
        {
            try
            {
                return Ok(await _usersManager.Create(user));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while creating user: {ex.Message}");
            }
        }

        // Update user 
        [HttpPut]
        [Route("api/users")]
        public async Task<IActionResult> Update([FromBody] User user)
        {
            try
            {
                return Ok(await _usersManager.Update(user));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating user: {ex.Message}");
            }
        }

        // Delete user by ID
        [HttpDelete]
        [Route("api/users/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _usersManager.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while deleting user: {ex.Message}");
            }
        }

    }
}