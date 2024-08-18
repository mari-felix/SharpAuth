using Microsoft.EntityFrameworkCore;
using API.Models;

namespace API.Context
{
    public class UserContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("server=localhost;port=3306;database=users;user=root;password=root;", ServerVersion.AutoDetect("server=localhost;port=3306;database=users;user=root;password=root;"));
        }

        public DbSet<User> Users { get; set;}
    }
}