using Microsoft.EntityFrameworkCore;
using API.Models;

namespace API.Context
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options) {   }

        public DbSet<User> Users { get; set;}
    }
}