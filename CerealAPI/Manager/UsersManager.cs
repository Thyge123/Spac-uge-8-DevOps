using CerealAPI.DbContext;
using CerealAPI.Model;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Generators;

namespace CerealAPI.Manager
{
    public class UsersManager
    {
        public readonly DBContext _dbContext;

        public UsersManager(DBContext dBContext)
        {
            _dbContext = dBContext;
        }

        // Get all users
        public Task<List<User>> GetAllAsync()
        {
            try
            {
                return _dbContext.Users.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving users: {ex.Message}");
                return null;
            }
        }

        // Get user by ID
        public Task<User?> Get(int id)
        {
            try
            {
                return _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving user: {ex.Message}");
                return null;
            }
        }

        // Get user by username
        public Task<User?> GetByUsername(string username)
        {
            try
            {
                return _dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving user: {ex.Message}");
                return null;
            }
        }


        // Get user by role
        public Task<User?> GetByRole(string role)
        {
            try
            {
                return _dbContext.Users.FirstOrDefaultAsync(u => u.Role == role);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving user: {ex.Message}");
                return null;
            }
        }

        // Create a new user
        public async Task<User> Create(User user)
        {
            try
            {
                HashPassword(user);
                await _dbContext.Users.AddAsync(user);
                await _dbContext.SaveChangesAsync();
                return user;
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Failed to create user due to database error", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating user: {ex.Message}");
                return null;
            }
        }

        // Update a user
        public async Task<User> Update(User user)
        {
            try
            {
                _dbContext.Users.Update(user);
                await _dbContext.SaveChangesAsync();
                return user;
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Failed to update user due to database error", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating user: {ex.Message}");
                return null;
            }
        }

        // Delete a user
        public async Task Delete(int id)
        {
            try
            {
                var user = await Get(id);
                if (user != null)
                {
                    _dbContext.Users.Remove(user);
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting user: {ex.Message}");
            }
        }

        // Hash password
        public void HashPassword(User user)
        {
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password); // Hash the password before storing it
        }

        // Verify password
        public bool VerifyPassword(User user, string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, user.Password); // Verify the password when user logs in
        }

    }
}
