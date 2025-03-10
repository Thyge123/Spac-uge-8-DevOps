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

        public Task<List<User>> GetAllAsync()
        {
            return _dbContext.Users.ToListAsync();
        }

        public Task<User> Get(int id)
        {
            return _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public Task<User> GetByUsername(string username)
        {
            return _dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public Task<User> GetByRole(string role)
        {
            return _dbContext.Users.FirstOrDefaultAsync(u => u.Role == role);
        }

        public async Task<User> Create(User user)
        {
            HashPassword(user);
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }

        public async Task<User> Update(User user)
        {
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }

        public async Task Delete(int id)
        {
            var user = await Get(id);
            if (user != null)
            {
                _dbContext.Users.Remove(user);
                await _dbContext.SaveChangesAsync();
            }
        }

        public void HashPassword(User user)
        {
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
        }

        public bool VerifyPassword(User user, string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, user.Password);
        }

    }
}
