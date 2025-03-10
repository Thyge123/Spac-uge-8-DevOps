using CerealAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace CerealAPI.DbContext
{
    public class DBContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {
        }
        public DbSet<Cereal> Cereals { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
