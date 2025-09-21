using Microsoft.EntityFrameworkCore;
using Usuarios.Domain.Entities;

namespace Usuarios.Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

}