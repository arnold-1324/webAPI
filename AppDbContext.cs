using Microsoft.EntityFrameworkCore;
using twitterclone.Models;

namespace twitterclone;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
}
